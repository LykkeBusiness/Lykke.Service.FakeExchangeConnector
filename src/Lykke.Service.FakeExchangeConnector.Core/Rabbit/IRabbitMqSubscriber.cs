using System;
using System.Threading.Tasks;

namespace Lykke.Service.FakeExchangeConnector.Core.Rabbit
{
    public interface IRabbitMqSubscriber<T>
    {
        void Subscribe(Func<T, Task> handleMessage);
    }
}
