using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Services.Caches
{
    public class OrderBookCache: GenericDictionaryCache<OrderBook>, IOrderBookCache
    {
        
    }
}
