using Lykke.Service.FakeExchangeConnector.Core.Domain;

namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IQuoteCache : IGenericDoubleDictionaryCache<ExchangeInstrumentQuote>
    {
        
    }
}
