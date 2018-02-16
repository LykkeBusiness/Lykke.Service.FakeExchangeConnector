namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IDoubleKeyedObject
    {
        string GetPartitionKey { get; }
        string GetRowKey { get; }
    }
}
