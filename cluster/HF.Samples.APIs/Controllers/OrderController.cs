﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using HF.Samples.OrderService;
using HF.Samples.StorageService;

namespace HF.Samples.APIs.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        /// <summary>
		/// Creating order from product.
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		[Route("create")]
        [HttpPost]
        public IActionResult Create([FromBody]string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest();

            var jobId = BackgroundJob.Enqueue<IOrderService>(x => x.CreateOrder(productId));

            BackgroundJob.ContinueWith<IInventoryService>(jobId, x => x.Reduce(productId));

            return Ok(new { Status = 1, Message = $"Enqueued successfully, ProductId->{productId}" });
        }
    }
}
