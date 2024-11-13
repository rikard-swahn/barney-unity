using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions {

    public static string toDebugString<TKey, TValue> (this IDictionary<TKey, TValue> dictionary)
    {
        return "{" + string.Join(",", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";
    }

    public static V PutIfAbsent<K, V>(this IDictionary<K, V> dict, K key, V defaultValue = default) {
        if (!dict.ContainsKey(key)) {
            dict[key] = defaultValue;
        }

        return dict[key];
    }      
}