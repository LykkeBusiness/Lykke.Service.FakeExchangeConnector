using System.Collections.Generic;

namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Configuration
{
    public interface IExchangeConfiguration
    {
        bool Enabled { get; set; }

        bool PubQuotesToRabbit { get; set; }

        IReadOnlyCollection<CurrencySymbol> SupportedCurrencySymbols { get; }
    }
}
