using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Services
{
    public interface ITradingService
    {
        Task<ExecutionReport> CreateOrder(string exchangeName, string instrument, TradeType tradeType,
            decimal price, decimal volume, bool isPublishToRabbit = true);

        bool? GetAcceptOrder(string exchangeName);
        bool? SetAcceptOrder(string exchangeName, bool acceptOrder);
        bool? GetPushEventToRabbit(string exchangeName);
        bool? SetPushEventToRabbit(string exchangeName, bool pushEventToRabbit);
    }
}
