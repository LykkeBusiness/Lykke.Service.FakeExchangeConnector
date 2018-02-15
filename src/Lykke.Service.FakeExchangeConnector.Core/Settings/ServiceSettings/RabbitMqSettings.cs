namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class RabbitMqSettings
    {
        public RabbitMQConnectionParams ExchangeConnectorQuotes { get; set; }
        
        public RabbitMQConnectionParams ExchangeConnectorOrder { get; set; }
        
        public RabbitMQConnectionParams FakeOrderBook { get; set; }
    }
}
