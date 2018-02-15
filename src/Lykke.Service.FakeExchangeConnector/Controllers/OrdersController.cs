using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IExchangeCache _exchangeCache;
        private readonly ITradingService _tradingService;

        public OrdersController(IExchangeCache exchangeCache,
            ITradingService tradingService)
        {
            _exchangeCache = exchangeCache;
            _tradingService = tradingService;
        }

        /// <summary>
        /// Returns information about the earlier placed order
        /// </summary>
        /// <param name="id">The order id</param>
        /// <param name="instrument">The instrument name of the order</param>
        /// <param name="exchangeName">The exchange name</param>
        [SwaggerOperation("GetOrder")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExecutionReport), 200)]
        public async Task<IActionResult> GetOrder(string id, [FromQuery, Required] string exchangeName, [FromQuery, Required] string instrument)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Places a new order on the exchange
        /// </summary>
        /// <param name="orderModel">A new order</param>
        /// <remarks>In the location header of successful response placed an URL for getting info about the order</remarks>
        [SwaggerOperation("CreateOrder")]
        [HttpPost]
        [ProducesResponseType(typeof(ExecutionReport), 200)]
        public async Task<IActionResult> Post([FromBody] OrderModel orderModel)
        {
            if (string.IsNullOrEmpty(orderModel?.ExchangeName)
                || string.IsNullOrEmpty(orderModel.Instrument)
                || orderModel.Volume == 0)
                return BadRequest("Bad model");

            if (!(_exchangeCache.Get(orderModel.ExchangeName)?.AcceptOrder ?? true))
                return BadRequest($"AcceptOrder is false for {orderModel.ExchangeName}");
            
            var result = _tradingService.CreateOrder(orderModel.ExchangeName, orderModel.Instrument,
                orderModel.TradeType, orderModel.Price ?? 0, orderModel.Volume);

            if (result == null)
                return BadRequest("No such exchange in config");

            return Ok();
        }

        private static string GetUniqueOrderId(OrderModel orderModel)
        {
            return orderModel.ExchangeName + DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Cancels the existing order
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">The order id to cancel</param>
        /// <param name="exchangeName">The exchange name</param>
        [SwaggerOperation("CancelOrder")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ExecutionReport), 200)]
        public async Task<IActionResult> CancelOrder(string id, [FromQuery, Required]string exchangeName)
        {
            throw new NotImplementedException();
        }
    }
}
