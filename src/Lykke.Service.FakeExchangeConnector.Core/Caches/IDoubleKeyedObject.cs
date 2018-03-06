namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IDoubleKeyedObject
    {
        string PartitionKey { get; }
        string RowKey { get; }
    }
}
