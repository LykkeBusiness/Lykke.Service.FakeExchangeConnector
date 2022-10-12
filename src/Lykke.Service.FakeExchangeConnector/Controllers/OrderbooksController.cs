using System;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public class OrderbooksController : BaseApiController
    {
        private readonly IOrderBookCache _orderBookCache;
        private readonly IOrderBookService _orderBookService;
        private readonly ILog _log;

        public OrderbooksController(IOrderBookCache orderBookCache, IOrderBookService orderBookService, ILog log)
        {
            _orderBookCache = orderBookCache;
            _orderBookService = orderBookService;
            _log = log;
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
        public IActionResult Post([FromBody] OrderBook[] orderbooks)
        {
            if (orderbooks == null || orderbooks.Length == 0)
                return BadRequest();

            _orderBookCache.SetAll(orderbooks);

            return Ok();
        }

        /// <summary>
        /// Remove order books related to particular asset pair
        /// </summary>
        /// <param name="asset">The asset pair id</param>
        /// <returns></returns>
        [SwaggerOperation("RemoveByAssetPairId")]
        [HttpDelete("{asset}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Delete(string asset)
        {
            try
            {
                _orderBookService.RemoveOrderBooksByAssetPair(asset);
            }
            catch (ArgumentNullException e)
            {
                _log.WriteError($"{nameof(OrderbooksController)}.{nameof(Delete)}", asset, e);

                return BadRequest();
            }

            return Ok();
        }
    }
}
