using System.Collections.Generic;

namespace MarkovInu.Markov
{
    public static class DictionaryExtensions
    {
        public static TValue AddOrGetExisting<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue value;

            if (!dictionary.TryGetValue(key, out value))
            {
                value = new TValue();

                dictionary.Add(key, value);
            }

            return value;
        }
    }
}
