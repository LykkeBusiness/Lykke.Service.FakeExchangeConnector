using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.FakeExchangeConnector.Core.Caches;
using MoreLinq;

namespace Lykke.Service.FakeExchangeConnector.Services.Caches
{
    /// <summary>
    /// Immutable thread-safe generic cache based on dictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericDictionaryCache<T> : IGenericDictionaryCache<T>
        where T: class, IKeyedObject, ICloneable
    {
        protected Dictionary<string, T> _cache;

        protected static readonly object LockObj = new object();

        protected GenericDictionaryCache()
        {
            ClearAll();
        }
        
        public T Get(string key)
        {
            lock (LockObj)
            {
                return _cache.TryGetValue(key, out var value)
                    ? (T) value.Clone()
                    : null;
            }
        }

        public IReadOnlyList<T> GetAll()
        {
           lock(LockObj)
           {
               return _cache.Values.Select(x => (T)x.Clone()).ToList();
           }
        }

        public void Set(T item)
        {
            if (item == null)
                return;
            
            lock (LockObj)
            {
                _cache[item.Key] = (T)item.Clone();
            }
        }

        public void SetAll(IEnumerable<T> items)
        {
            if (items == null)
                return;
            
            lock (LockObj)
            {
                items.Where(x => x != null).ForEach(x => _cache[x.Key] = (T)x.Clone());
            }
        }

        public void Clear(string key)
        {
            lock (LockObj)
            {
                _cache.Remove(key);
            }
        }

        public void ClearByCondition(Func<T, bool> removalPredicate)
        {
            if (removalPredicate == null) return;
            
            var removals = new List<string>();
                
            lock (LockObj)
            {
                _cache.ForEach(pair =>
                {
                    if (removalPredicate(pair.Value))
                    {
                        removals.Add(pair.Key);
                    }
                });
                
                removals.ForEach(key => _cache.Remove(key));
            }
        }

        public void ClearAll()
        {
            lock (LockObj)
            {
                _cache = new Dictionary<string, T>();
            }
        }

        public void Initialize(IEnumerable<T> items)
        {
            ClearAll();
            SetAll(items);
        }
    }
}
