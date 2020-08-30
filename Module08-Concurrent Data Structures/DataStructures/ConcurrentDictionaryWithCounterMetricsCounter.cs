using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DataStructures
{
    public class ConcurrentDictionaryWithCounterMetricsCounter : IMetricsCounter
    {
        // Implement this class using ConcurrentDictionary and the provided AtomicCounter class.
        // AtomicCounter should be created only once per key, then its Increment method should be used.

        private readonly ConcurrentDictionary<string, AtomicCounter> _dict = new ConcurrentDictionary<string, AtomicCounter>();
        
        public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
        {
            using var enumerator = _dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var kvp = enumerator.Current;
                yield return new KeyValuePair<string, int>(kvp.Key, kvp.Value.Count);
            }
        }

        public void Increment(string key)
        {
            var ac = _dict.GetOrAdd(key, new AtomicCounter());
            ac.Increment();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public class AtomicCounter
        {
            int value;

            public void Increment() => Interlocked.Increment(ref value);

            public int Count => Volatile.Read(ref value);
        }
    }
}