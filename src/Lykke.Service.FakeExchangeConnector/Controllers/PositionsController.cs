using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public sealed class PositionsController : BaseApiController
    {
        private readonly IExchangeCache _exchangeCache;
        
        public PositionsController(IExchangeCache exchangeCache)
        {
            _exchangeCache = exchangeCache;
        }

        /// <summary>
        /// Returns information about opened positions
        /// </summary>
        /// <param name="exchangeName">The exchange name</param>
        [SwaggerOperation("GetOpenedPosition")]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<PositionModel>), 200)]
        public async Task<IActionResult> Index([FromQuery, Required] string exchangeName)
        {
            return Ok(_exchangeCache.Get(exchangeName)?.Positions.Select(x => new PositionModel
            {
                Symbol = x.Symbol,
                PositionVolume = x.PositionVolume
            }));
        }
    }
}
