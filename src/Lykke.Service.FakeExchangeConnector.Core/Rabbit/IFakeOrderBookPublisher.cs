using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Rabbit
{
    public interface IFakeOrderBookPublisher
    {
        Task Publish(OrderBook orderBook);
    }
}
