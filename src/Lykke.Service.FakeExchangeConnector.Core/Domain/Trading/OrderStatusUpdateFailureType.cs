namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Trading
{
    public enum OrderStatusUpdateFailureType
    {
        None,
        Unknown,
        ExchangeError,
        ConnectorError,
        InsufficientFunds
    }
}
