using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;

namespace Lykke.Service.FakeExchangeConnector.RabbitPublishers
{
    public sealed class FakeOrderBookFakePublisher : RabbitMqPublisher<OrderBook>, IFakeOrderBookPublisher
    {
        public FakeOrderBookFakePublisher(string connectionString, 
            string exchangeName, 
            bool enabled, 
            ILog log,
            bool durable = false,
            string messageFormat = null) 
            : base(connectionString, exchangeName, enabled, log, durable, messageFormat)
        {
            
        }
        
        
    }
}
