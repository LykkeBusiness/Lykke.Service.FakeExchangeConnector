using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IExchangeCache : IGenericDictionaryCache<Exchange>
    {
        Exchange UpdatePosition(string exchangeName, string instrument, TradeType tradeType, decimal volume);
    }
}
