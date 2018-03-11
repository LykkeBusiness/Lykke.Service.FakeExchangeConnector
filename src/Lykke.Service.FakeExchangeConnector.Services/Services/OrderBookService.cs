using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using Lykke.Service.FakeExchangeConnector.Core.Domain.Trading;
using Lykke.Service.FakeExchangeConnector.Core.Rabbit;
using Lykke.Service.FakeExchangeConnector.Core.Services;
using Lykke.Service.FakeExchangeConnector.Core.Settings.ServiceSettings;

namespace Lykke.Service.FakeExchangeConnector.Services.Services
{
    public class OrderBookService : IOrderBookService
    {
        private readonly IOrderBookCache _orderBookCache;

        private readonly IFakeOrderBookPublisher _fakeOrderBookPublisher;

        private readonly FakeExchangeConnectorSettings _fakeExchangeConnectorSettings;

        public OrderBookService(IOrderBookCache orderBookCache,
            IFakeOrderBookPublisher fakeOrderBookPublisher,
            FakeExchangeConnectorSettings fakeExchangeConnectorSettings)
        {
            _orderBookCache = orderBookCache;

            _fakeOrderBookPublisher = fakeOrderBookPublisher;

            _fakeExchangeConnectorSettings = fakeExchangeConnectorSettings;
        }

        public async Task PostFakeOrderBooks()
        {
            var orderbooks = ShakeBooks(_orderBookCache.GetAll());

            await Task.WhenAll(orderbooks.Select(_fakeOrderBookPublisher.Publish));
        }

        private IEnumerable<OrderBook> ShakeBooks(IEnumerable<OrderBook> orderBooks)
        {
            var random = new Random();
            var delta = (decimal) random.NextDouble();
            return orderBooks.Select(x =>
                    new OrderBook(x.Source, x.AssetPairId, ShakePrices(x.Asks, delta), ShakePrices(x.Bids, delta), x.Timestamp))
                .ToList();
        }

        private IReadOnlyList<VolumePrice> ShakePrices(IEnumerable<VolumePrice> book, decimal delta)
        {
            return book.Select(x =>
                    new VolumePrice(Math.Round(x.Price *
                                               (1 + _fakeExchangeConnectorSettings.OrderBookDeltaPercentage * delta),
                        _fakeExchangeConnectorSettings.PriceAccuracy), x.Volume))
                .ToList();
        }
    }
}
