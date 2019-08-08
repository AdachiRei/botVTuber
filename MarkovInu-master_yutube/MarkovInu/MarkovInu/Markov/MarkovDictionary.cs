using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkovInu.Markov
{
    public class MarkovDictionary : IEnumerable<KeyValuePair<MarkovKey, List<string>>>
    {
        private const string Eos = "__END_OF_SENTENCE__";

        private readonly Random _random = new Random();
        private readonly Dictionary<MarkovKey, List<string>> _innerDictionary = new Dictionary<MarkovKey, List<string>>();

        private static readonly MarkovKey _startKey = new MarkovKey();

        public void AddSentence(string[] words)
        {
            var key = _startKey;

            foreach (var word in words)
            {
                _innerDictionary.AddOrGetExisting(key).Add(word);

                key = key.Push(word);
            }

            _innerDictionary.AddOrGetExisting(key).Add(Eos);
        }

        public IList<string> BuildSentence()
        {
            var result = new List<string>();

            var key = _startKey;

            while (true)
            {
                var list = _innerDictionary[key];

                var word = list[_random.Next(list.Count)];

                if (word == Eos)
                {
                    break;
                }

                result.Add(word);

                key = key.Push(word);
            }

            return result;
        }

        public IEnumerator<KeyValuePair<MarkovKey, List<string>>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
