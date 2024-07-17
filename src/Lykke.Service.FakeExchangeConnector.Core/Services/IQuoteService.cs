using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Domain;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Services
{
    public interface IQuoteService
    {
        ExchangeInstrumentQuote Get(string exchangeName, string instrument);
    }
}
