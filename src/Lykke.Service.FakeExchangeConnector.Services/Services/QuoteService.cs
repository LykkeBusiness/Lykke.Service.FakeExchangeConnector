using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.SettingsReader;

namespace Lykke.Service.FakeExchangeConnector.Services.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteCache _quoteCache;

        private readonly ILog _log;

        public QuoteService(IQuoteCache quoteCache,
            ILog log)
        {
            _quoteCache = quoteCache;
            _log = log;
        }

        /// <summary>
        /// Swaps 6-symbol instruments. just for simplicity.. for USDBTC
        /// </summary>
        /// <param name="instrument"></param>
        /// <returns></returns>
        private string SwapInstrument(string instrument)
        {
            return instrument.Length != 6 ? null : new string(instrument.Skip(3).Concat(instrument.Take(3)).ToArray());
        }

        public ExchangeInstrumentQuote Get(string exchangeName, string instrument)
        {
            return _quoteCache.Get(exchangeName, instrument);
        }

        private ExchangeBestPrice ConvertToBestPriceQuote(OrderBook orderBook)
        {
            var ask = GetBestPrice(true, orderBook.Asks);
            var bid = GetBestPrice(false, orderBook.Bids);
            
            return ask == null || bid == null
                ? null
                : new ExchangeBestPrice
            {
                ExchangeName = orderBook.Source,
                Instrument = orderBook.AssetPairId,
                Timestamp = orderBook.Timestamp,
                Ask = ask.Value,
                Bid = bid.Value
            };
        }

        private decimal? GetBestPrice(bool isBuy, IReadOnlyCollection<VolumePrice> prices)
        {
            if (!prices.Any())
                return null;
            return isBuy
                ? prices.Min(x => x.Price)
                : prices.Max(x => x.Price);
        }
    }
}
