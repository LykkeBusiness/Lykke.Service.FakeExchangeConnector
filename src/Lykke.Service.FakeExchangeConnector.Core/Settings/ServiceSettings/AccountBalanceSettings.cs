using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class AccountBalanceSettings
    {
        [Optional]
        public string Asset { get; set; }
        
        [Optional]
        public decimal Balance { get; set; }
    }
}
