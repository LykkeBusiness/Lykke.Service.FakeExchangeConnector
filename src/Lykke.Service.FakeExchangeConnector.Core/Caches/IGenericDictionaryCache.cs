using System;
using System.Collections.Generic;

namespace Lykke.Service.FakeExchangeConnector.Core.Caches
{
    public interface IGenericDictionaryCache<T>
    {
        T Get(string key);

        IReadOnlyList<T> GetAll();

        void Set(T item);

        void SetAll(IEnumerable<T> items);

        void Clear(string key);

        void ClearByCondition(Func<T, bool> removalPredicate);

        void ClearAll();

        void Initialize(IEnumerable<T> items);
    }
}
