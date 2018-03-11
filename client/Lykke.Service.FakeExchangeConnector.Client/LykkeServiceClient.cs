using System;
using Common.Log;

namespace Lykke.Service.FakeExchangeConnector.Client
{
    public class FakeExchangeConnectorClient : IFakeExchangeConnectorClient, IDisposable
    {
        private readonly ILog _log;

        public FakeExchangeConnectorClient(string serviceUrl, ILog log)
        {
            _log = log;
        }

        public void Dispose()
        {
            //if (_service == null)
            //    return;
            //_service.Dispose();
            //_service = null;
        }
    }
}
