using Common.Log;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;

namespace Lykke.Service.FakeExchangeConnector.RabbitPublishers
{
    public sealed class ExecutionReportPublisher : RabbitMqPublisher<ExecutionReport>, IExecutionReportPublisher
    {
        public ExecutionReportPublisher(string connectionString, 
            string exchangeName, 
            bool enabled, 
            ILog log,
            bool durable = true) 
            : base(connectionString, exchangeName, enabled, log, durable)
        {
            
        }
        
        
    }
}
