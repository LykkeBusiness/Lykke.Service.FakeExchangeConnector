using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.RabbitSubscribers
{
    public class OrderBookSubscriber : RabbitMqSubscriber<OrderBook>
    {
        public OrderBookSubscriber(ILog log, string connectionString, string exchangeName, string queueName, bool isDurable)
            : base(log, connectionString, exchangeName, queueName, isDurable)
        {
            
        }
    }
}
