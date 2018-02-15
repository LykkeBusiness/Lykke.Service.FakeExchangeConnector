using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IOrderBookCache : IGenericDictionaryCache<OrderBook>
    {
        
    }
}
