namespace Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings
{
    public class StreamingSupportSettings
    {
        /// <summary>
        /// Can stream order books
        /// </summary>
        public bool OrderBooks { get; set; }

        /// <summary>
        /// Can stream orders updates
        /// </summary>
        public bool Orders { get; set; }
    }
}
