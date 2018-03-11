using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class ExchangeConfig
    {
        public string Name { get; set; }
        
        [Optional]
        public AccountBalance[] Accounts { get; set; }
        
        public PositionSettings[] Positions { get; set; }

        public string[] Instruments { get; set; }
        
        public StreamingSupportSettings StreamingSupport { get; set; }

        public bool AcceptOrder { get; set; }
        
        public bool PushEventToRabbit { get; set; }
    }
}
