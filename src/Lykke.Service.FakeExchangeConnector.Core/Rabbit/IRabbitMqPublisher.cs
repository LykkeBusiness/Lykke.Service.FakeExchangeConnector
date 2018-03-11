using System.Threading.Tasks;

namespace Lykke.Service.FakeExchangeConnector.Core.Rabbit
{
    public interface IRabbitMqPublisher<in T>
    {
        Task Publish(T message);
    }
}
