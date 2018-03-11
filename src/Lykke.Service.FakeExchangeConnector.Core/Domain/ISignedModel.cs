namespace Lykke.Service.FakeExchangeConnector.Core.Domain
{
    public interface ISignedModel
    {
        string GetStringToSign();
    }
}