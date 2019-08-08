using System;
using System.Linq;

namespace MarkovInu.Markov
{
    public sealed class MarkovKey : IEquatable<MarkovKey>
    {
        public MarkovKey()
        {
            _keys = new string[N];
        }

        private MarkovKey(params string[] keys)
        {
            _keys = keys;
        }

        private const int N = 1;
        private const string DefaultKeySeparator = ":";

        private readonly string[] _keys;

        public MarkovKey Push(string key)
        {
            var newKeys = new string[N];

            newKeys[0] = key;

            for (int i = 0; i < N - 1; i++)
            {
                newKeys[i + 1] = _keys[i];
            }

            return new MarkovKey(newKeys);
        }

        public override int GetHashCode()
        {
            return _keys.Aggregate(int.MaxValue, (current, t) => current ^ (t ?? "").GetHashCode());
        }

        public override string ToString()
        {
            return string.Join(DefaultKeySeparator, _keys);
        }

        public override bool Equals(object obj)
        {
            var key = obj as MarkovKey;

            if (key == null)
            {
                return false;
            }

            return Equals(key);
        }

        public bool Equals(MarkovKey other)
        {
            if (_keys.Length != other._keys.Length)
            {
                return false;
            }

            return !_keys.Where((t, i) => t != other._keys[i]).Any();
        }
    }
}
