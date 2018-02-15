using System.Threading.Tasks;

namespace Lykke.Service.FakeExchangeConnector.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
