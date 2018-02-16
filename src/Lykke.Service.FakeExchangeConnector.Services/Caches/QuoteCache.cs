using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;

namespace Lykke.Service.FakeExchangeConnector.Services.Caches
{
    public class QuoteCache : GenericDoubleDictionaryCache<ExchangeInstrumentQuote>, IQuoteCache
    {
        
    }
}
