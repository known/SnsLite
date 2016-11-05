using System.Collections;

namespace Known.Inners
{
    sealed class DataCache
    {
        private static Hashtable cached = new Hashtable();

        public static void Set<T>(string key, T value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
                return;

            if (!cached.ContainsKey(key))
            {
                lock (cached.SyncRoot)
                {
                    cached[key] = value;
                }
            }
        }

        public static T Get<T>(string key)
        {
            if (cached.ContainsKey(key))
            {
                return (T)cached[key];
            }

            return default(T);
        }
    }
}
