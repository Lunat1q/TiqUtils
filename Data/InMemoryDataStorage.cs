using System.Collections.Generic;
using TiqUtils.TypeSpeccific;

namespace TiqUtils.Data
{
    public static class InMemoryDataStorage
    {
        private static readonly Dictionary<string, object> Storage = new Dictionary<string, object>();

        public static string Store<T>(T data)
        {
            var newKey = StringUtils.RandomString(16);
            if (Storage.ContainsKey(newKey))
                return Store(data);
            Storage.Add(newKey, data);
            return newKey;
        }

        public static T GetData<T>(string key)
        {
            if (Storage.ContainsKey(key))
                return (T)Storage[key];
            return default(T);
        }
    }
}
