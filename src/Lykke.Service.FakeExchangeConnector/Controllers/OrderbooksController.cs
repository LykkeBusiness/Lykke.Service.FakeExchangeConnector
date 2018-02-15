using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public class OrderbooksController : BaseApiController
    {
        private readonly IOrderBookCache _orderBookCache;

        public OrderbooksController(IOrderBookCache orderBookCache)
        {
            _orderBookCache = orderBookCache;
        }
        
        /// <summary>
        /// Get a list of all orderbooks from cache
        /// </summary>
        [HttpGet]
        [SwaggerOperation("GetAll")]
        [ProducesResponseType(200)]
        public IActionResult List()
        {
            return Ok(_orderBookCache.GetAll());
        }
        
        /// <summary>
        /// Orderbook manual input
        /// </summary>
        /// <param name="orderbooks"></param>
        [SwaggerOperation("ManualInput")]
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromBody] OrderBook[] orderbooks)
        {
            if (orderbooks == null || orderbooks.Length == 0)
                return BadRequest();

            _orderBookCache.SetAll(orderbooks);

            return Ok();
        }
    }
}
