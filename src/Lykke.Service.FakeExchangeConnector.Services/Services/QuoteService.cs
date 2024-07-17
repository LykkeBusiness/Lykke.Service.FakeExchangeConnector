using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Services;

namespace Lykke.Service.FakeExchangeConnector.Services.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteCache _quoteCache;
        public QuoteService(IQuoteCache quoteCache)
        {
            _quoteCache = quoteCache;
        }
        public ExchangeInstrumentQuote Get(string exchangeName, string instrument)
        {
            return _quoteCache.Get(exchangeName, instrument);
        }
    }
}
