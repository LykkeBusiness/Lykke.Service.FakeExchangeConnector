using System.Collections.Generic;
using System.Linq;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public class ExchangesController : BaseApiController
    {
        private readonly IExchangeCache _exchangeCache;

        public ExchangesController(IExchangeCache exchangeCache)
        {
            _exchangeCache = exchangeCache;
        }
        
        /// <summary>
        /// Get a list of all connected exchanges
        /// </summary>
        /// <remarks>The names of available exchanges participates in API calls for exchange-specific methods</remarks>
        [HttpGet]
        [SwaggerOperation("GetSupportedExchanges")]
        public IEnumerable<string> List()
        {
            return _exchangeCache.GetAll().Select(x => x.Name);
        }

        /// <summary>
        /// Get information about a specific exchange
        /// </summary>
        /// <param name="exchangeName">Name of the specific exchange</param>
        [SwaggerOperation("GetExchangeInfo")]
        [HttpGet("{exchangeName}")]
        [ProducesResponseType(typeof(ExchangeInformationModel), 200)]
        public IActionResult Index(string exchangeName)
        {
            var result = _exchangeCache.Get(exchangeName);

            if (result == null)
                return BadRequest($"Invalid {nameof(exchangeName)}");

            return Ok(new ExchangeInformationModel
            {
                Instruments = result.Instruments,
                Name = result.Name,
                State = ExchangeState.Connected,
                StreamingSupport = result.StreamingSupport
            });
        }
    }
}
