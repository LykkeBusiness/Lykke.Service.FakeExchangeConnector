using JetBrains.Annotations;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;
using Lykke.Service.FakeExchangeConnector.Core.Settings.SlackNotifications;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.FakeExchangeConnector.Core.Settings
{
    public class AppSettings
    {
        public FakeExchangeConnectorSettings FakeExchangeConnectorService { get; set; }
        [Optional, CanBeNull]
        public SlackNotificationsSettings SlackNotifications { get; set; }
    }
}
