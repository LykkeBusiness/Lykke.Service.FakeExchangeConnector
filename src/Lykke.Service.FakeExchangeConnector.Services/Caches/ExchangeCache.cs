using System.Linq;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Services.Caches
{
    public class ExchangeCache : GenericDictionaryCache<Exchange>, IExchangeCache
    {
        public Exchange UpdatePosition(string exchangeName, string instrument, TradeType tradeType, decimal volume)
        {
            lock (LockObj)
            {
                var exchange = _cache.TryGetValue(exchangeName, out var value) ? value : null;

                if (exchange == null)
                    return null;

                var position = exchange.Positions.FirstOrDefault(x => x.Symbol == instrument) ?? new Position
                {
                    Symbol = instrument
                };
                
                exchange.Positions = exchange.Positions.Except(new [] {position}).ToList();
                
                position.PositionVolume += volume * (tradeType == TradeType.Buy ? 1 : -1);

                exchange.Positions = exchange.Positions.Concat(new[] {position}).ToList();
                
                return exchange;
            }
        }
    }
}
