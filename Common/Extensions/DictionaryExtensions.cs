using Common.Options;
using System.Collections.Generic;

namespace Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
            dictionary.TryGetValue(key, out var value) ? (Option<TValue>)value : Nothing.Value;
    }
}
