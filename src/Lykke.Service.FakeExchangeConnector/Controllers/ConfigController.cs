using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public class ConfigController : BaseApiController
    {
        private readonly FakeExchangeConnectorSettings _fakeExchangeConnectorSettings;

        private readonly ITradingService _tradingService;
        
        public ConfigController(FakeExchangeConnectorSettings fakeExchangeConnectorSettings,
            ITradingService tradingService)
        {
            _fakeExchangeConnectorSettings = fakeExchangeConnectorSettings;
            
            _tradingService = tradingService;
        }
        
        #region PublishingPeriod
        
        /// <summary>
        /// Get orderbook publishing period in milliseconds
        /// </summary>
        [SwaggerOperation("GetOrderBookPublishingPeriod")]
        [HttpGet("getOrderBookPubPeriod")]
        [ProducesResponseType(typeof(int), 200)]
        public IActionResult GetOrderBookPublishingPeriod()
        {
            return Ok(_fakeExchangeConnectorSettings.FakeOrderBookPublishingPeriodMilliseconds);
        }
        
        #endregion

        #region DeltaPercentage

        /// <summary>
        /// Get orderbook delta in percentage
        /// </summary>
        [SwaggerOperation("GetOrderBookDeltaPercentage")]
        [HttpGet("getOrderBookDelta")]
        [ProducesResponseType(typeof(decimal), 200)]
        public IActionResult GetOrderBookDeltaPercentage()
        {
            return Ok(_fakeExchangeConnectorSettings.OrderBookDeltaPercentage);
        }
        
        /// <summary>
        /// Set orderbook delta in percentage
        /// </summary>
        [SwaggerOperation("SetOrderBookDeltaPercentage")]
        [HttpPost("setOrderBookDelta")]
        [ProducesResponseType(typeof(decimal), 200)]
        public IActionResult SetOrderBookDeltaPercentage([FromQuery] decimal delta)
        {
            if (delta < 0 || delta > 1)
                return BadRequest("Incorrect parameter, must be 0 <= delta <= 1.");

            _fakeExchangeConnectorSettings.OrderBookDeltaPercentage = delta;
            return Ok();
        }

        #endregion

        #region PriceAccuracy

        /// <summary>
        /// Get output price accuracy
        /// </summary>
        [SwaggerOperation("GetPriceAccuracy")]
        [HttpGet("getPriceAccuracy")]
        [ProducesResponseType(typeof(int), 200)]
        public IActionResult GetPriceAccuracy()
        {
            return Ok(_fakeExchangeConnectorSettings.PriceAccuracy);
        }
        
        /// <summary>
        /// Set output price accuracy
        /// </summary>
        [SwaggerOperation("SetPriceAccuracy")]
        [HttpPost("setPriceAccuracy")]
        [ProducesResponseType(typeof(int), 200)]
        public IActionResult SetPriceAccuracy([FromQuery] int priceAccuracy)
        {
            if (priceAccuracy < 0 || priceAccuracy > 10)
                return BadRequest("Incorrect parameter, must be 0 <= priceAccuracy <= 10.");

            _fakeExchangeConnectorSettings.PriceAccuracy = priceAccuracy;
            return Ok();
        }

        #endregion

        #region AcceptOrder

        /// <summary>
        /// Get if particular exchange accepts orders
        /// </summary>
        [SwaggerOperation("GetAcceptOrder")]
        [HttpGet("getAcceptOrder")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetAcceptOrder([FromQuery] string exchangeName)
        {
            var result = _tradingService.GetAcceptOrder(exchangeName);

            if (result == null)
                return NotFound();
            
            return Ok(result.Value);
        }
        
        /// <summary>
        /// Set if particular exchange accepts orders
        /// </summary>
        [SwaggerOperation("SetAcceptOrder")]
        [HttpPost("setAcceptOrder")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(404)]
        public IActionResult SetAcceptOrder([FromQuery] string exchangeName, [FromQuery] bool acceptOrder)
        {
            var result = _tradingService.SetAcceptOrder(exchangeName, acceptOrder);
            
            if (result == null)
                return NotFound();
            
            return Ok();
        }

        #endregion

        #region PushEventToRabbit

        /// <summary>
        /// Get if orderbooks of particular exchange are sent to rabbit
        /// </summary>
        [SwaggerOperation("GetPushEventToRabbit")]
        [HttpGet("getPushEventsToRabbit")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetPushEventToRabbit([FromQuery] string exchangeName)
        {
            var result = _tradingService.GetPushEventToRabbit(exchangeName);

            if (result == null)
                return NotFound();
            
            return Ok(result.Value);
        }
        
        /// <summary>
        /// Set if orderbooks of particular exchange are sent to rabbit
        /// </summary>
        [SwaggerOperation("SetPushEventToRabbit")]
        [HttpPost("setPushEventsToRabbit")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(404)]
        public IActionResult SetPushEventToRabbit([FromQuery] string exchangeName, [FromQuery] bool pushEventToRabbit)
        {
            var result = _tradingService.SetPushEventToRabbit(exchangeName, pushEventToRabbit);
            
            if (result == null)
                return NotFound();
            
            return Ok();
        }

        #endregion
    }
}
