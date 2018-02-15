using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class RabbitMQConnectionParams
    {
        [Optional]
        public string ConnectionString { get; set; }

        [Optional]
        public string ExchangeName { get; set; }

        [Optional]
        public string ExchangeType => "fanout";

        [Optional]
        public string QueueName { get; set; }

        [Optional]
        public string RoutingKey => null;
    }
}
