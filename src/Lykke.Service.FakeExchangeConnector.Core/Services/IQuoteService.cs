using Lykke.Service.FakeExchangeConnector.Core.Domain;

namespace Lykke.Service.FakeExchangeConnector.Core.Services
{
    public interface IQuoteService
    {
        ExchangeInstrumentQuote Get(string exchangeName, string instrument);
    }
}
