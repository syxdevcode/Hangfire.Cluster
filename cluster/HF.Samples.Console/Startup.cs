using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Hangfire;
using Autofac.Extensions.DependencyInjection;
using Hangfire.Samples.Framework;
using System.Reflection;
using HF.Samples.GoodsService;
using HF.Samples.OrderService;
using HF.Samples.StorageService;
using Hangfire.Dashboard;
using Hangfire.Console;

namespace HF.Samples.Console
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHangfire(x =>
            {
                var connectionString = Configuration.GetConnectionString("hangfire.redis");
                x.UseRedisStorage(connectionString);
            });

            return RegisterAutofac(services);
        }

        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterModule(new DelegateModule(() => new Assembly[]
            {
                typeof(IProductService).GetTypeInfo().Assembly,
                typeof(IOrderService).GetTypeInfo().Assembly,
                typeof(IInventoryService).GetTypeInfo().Assembly
            }));

            var container = builder.Build();

            GlobalConfiguration.Configuration.UseAutofacActivator(container);

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            var options = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };
            app.UseHangfireDashboard("/hangfire", options);

        }
    }

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        //这里需要配置权限规则
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
