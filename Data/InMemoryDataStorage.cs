using System.Collections.Generic;
using TiqUtils.TypeSpeccific;

namespace TiqUtils.Data
{
    public static class InMemoryDataStorage
    {
        private static readonly Dictionary<string, object> Storage = new Dictionary<string, object>();

        public static string Store<T>(this T data)
        {
            var newKey = StringUtils.RandomString(16);
            if (Storage.ContainsKey(newKey))
                return Store(data);
            Storage.Add(newKey, data);
            return newKey;
        }

        public static T GetData<T>(string key, bool deleteData = true)
        {
            if (!Storage.ContainsKey(key)) return default(T);
            var data = (T) Storage[key];
            if (deleteData)
                Storage.Remove(key);
            return data;
        }
    }
}
