using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.FakeExchangeConnector.Controllers
{
    public sealed class AccountController : BaseApiController
    {
        private readonly IExchangeCache _exchangeCache;

        public AccountController(IExchangeCache exchangeCache)
        {
            _exchangeCache = exchangeCache;
        }

        /// <summary>
        /// Returns full balance information on the exchange
        /// </summary>
        /// <param name="exchangeName">The exchange name</param>
        /// <returns></returns>
        [SwaggerOperation("GetTradeBalance")]
        [HttpGet("tradeBalance")]
        [ProducesResponseType(typeof(IReadOnlyCollection<TradeBalanceModel>), 200)]
        public async Task<IActionResult> GetTradeBalance([FromQuery]string exchangeName)
        {
            return Ok(_exchangeCache.Get(exchangeName)?.Accounts.Select(x => new TradeBalanceModel
            {
                AccountCurrency = x.Asset,
                Totalbalance = x.Balance,
                MarginUsed = 0,
                MaringAvailable = 100000,
                UnrealisedPnL = 0
            }));
        }
    }
}
