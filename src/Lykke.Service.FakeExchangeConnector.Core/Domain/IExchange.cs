using System.Collections.Generic;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;

namespace Lykke.Service.FakeExchangeConnector.Core.Domain
{
    public interface IExchange
    {
        string Name { get; }

        IReadOnlyList<Instrument> Instruments { get; }
    }
}
