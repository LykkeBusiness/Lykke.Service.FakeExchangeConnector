namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Trading
{
    public enum AcknowledgementFailureType
    {
        None,
        Unknown,
        ExchangeError,
        ConnectorError,
        InsufficientFunds
    }
}
