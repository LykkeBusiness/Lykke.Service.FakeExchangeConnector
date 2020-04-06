using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using MessagePack;
using Newtonsoft.Json;

namespace Lykke.Service.FakeExchangeConnector.Core.Domain.Trading
{
    [MessagePackObject]
    public sealed class OrderBook : IKeyedObject, ICloneable
    {
        public OrderBook()
        {
            
        }
        
        public OrderBook(string source, string assetPairId, List<VolumePrice> asks, List<VolumePrice> bids, DateTime timestamp)
        {
            Source = source;
            AssetPairId = assetPairId;
            Asks = asks;
            Bids = bids;
            Timestamp = timestamp;
        }

        [JsonProperty("source"), Key(0)]
        public string Source { get; set; }

        [JsonProperty("asset"), Key(1)]
        public string AssetPairId { get; set; }

        [JsonProperty("timestamp"), Key(2)]
        public DateTime Timestamp { get; set; }

        [JsonProperty("asks"), Key(3)]
        public List<VolumePrice> Asks { get; set; }

        [JsonProperty("bids"), Key(4)]
        public List<VolumePrice> Bids { get; set; }

        [JsonIgnore, IgnoreMember]
        public string Key => $"{Source}_{AssetPairId}";

        public object Clone()
        {
            return new OrderBook(Source, AssetPairId, Asks, Bids, Timestamp);
        }
    }

    [MessagePackObject]
    public sealed class VolumePrice
    {
        public VolumePrice()
        {
            
        }
        
        public VolumePrice(decimal price, decimal volume)
        {
            Price =  price;
            Volume = Math.Abs(volume);
        }

        [JsonProperty("volume"), Key(0)]
        public decimal Volume { get; set; }
        
        [JsonProperty("price"), Key(1)]
        public decimal Price { get; set; }
    }
}
