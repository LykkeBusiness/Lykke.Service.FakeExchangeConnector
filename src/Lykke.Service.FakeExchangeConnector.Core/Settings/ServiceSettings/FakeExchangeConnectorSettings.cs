namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class FakeExchangeConnectorSettings
    {
        public ExchangeConfig[] ExchangesConfig { get; set; }
        
        public DbSettings Db { get; set; }

        public RabbitMqSettings Rabbit { get; set; }
        
        public int FakeOrderBookPublishingPeriodMilliseconds { get; set; }
        
        public decimal OrderBookDeltaPercentage { get; set; }
        
        public int PriceAccuracy { get; set; }
    }
}
