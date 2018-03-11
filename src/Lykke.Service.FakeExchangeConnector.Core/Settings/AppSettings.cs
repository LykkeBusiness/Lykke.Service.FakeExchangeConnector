using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;
using Lykke.Service.FakeExchangeConnector.Core.Settings.SlackNotifications;

namespace Lykke.Service.FakeExchangeConnector.Core.Settings
{
    public class AppSettings
    {
        public FakeExchangeConnectorSettings FakeExchangeConnectorService { get; set; }
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
