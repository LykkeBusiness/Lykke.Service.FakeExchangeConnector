using System.Collections.Generic;

namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Configuration
{
    public class BasicExchangeConfiguration : IExchangeConfiguration
    {
        public bool Enabled { get; set; }
        public bool PubQuotesToRabbit { get; set; }
        public IReadOnlyCollection<CurrencySymbol> SupportedCurrencySymbols { get; }
    }
}
