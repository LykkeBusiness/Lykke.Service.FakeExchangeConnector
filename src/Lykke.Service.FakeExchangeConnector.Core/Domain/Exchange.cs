using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Newtonsoft.Json;

namespace Lykke.Service.FakeExchangeConnector.Core.Domain
{
    public class Exchange : IExchange, IKeyedObject, ICloneable
    {
        public string Name { get; private set; }
        
        public IReadOnlyList<AccountBalance> Accounts { get; set; }
        
        public IReadOnlyList<Position> Positions { get; set; }

        public IReadOnlyList<Instrument> Instruments { get; set; }
        
        public StreamingSupport StreamingSupport { get; set; }
        
        public bool AcceptOrder { get; set; }
        
        public bool PushEventToRabbit { get; set; }

        [JsonIgnore]
        public string GetKey => Name;

        public Exchange(string name)
        {
            Name = name;
            Accounts = new List<AccountBalance>();
            Positions = new List<Position>();
            StreamingSupport = new StreamingSupport(false, false);
        }
        
        public object Clone()
        {
            return new Exchange(this.Name)
            {
                Accounts = this.Accounts?.Select(x => x.Clone()).ToList(),
                Positions = this.Positions?.Select(x => x.Clone()).ToList(),
                Instruments = this.Instruments?.Select(x => x.Clone()).ToList(),
                StreamingSupport = this.StreamingSupport,
                AcceptOrder = this.AcceptOrder,
                PushEventToRabbit = this.PushEventToRabbit
            };
        }

        public override string ToString()
        {
            return $"{Name}: {string.Join(", ", Positions.Select(x => x.Symbol))}";
        }
    }
}
