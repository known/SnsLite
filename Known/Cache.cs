using System.Collections.Generic;

namespace Known
{
    public interface ICache
    {
        T Get<T>(string key);
        void Remove(string key);
        void RemoveAll();
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, int expires);
    }

    public class Cache : ICache
    {
        private static Dictionary<string, object> cached = new Dictionary<string, object>();

        public T Get<T>(string key)
        {
            if (!string.IsNullOrWhiteSpace(key) && cached.ContainsKey(key))
            {
                return (T)cached[key];
            }

            return default(T);
        }

        public void Remove(string key)
        {
            if (cached.ContainsKey(key))
            {
                cached.Remove(key);
            }
        }

        public void RemoveAll()
        {
            cached.Clear();
        }

        public void Set<T>(string key, T value)
        {
            Check.Required(key, "key");

            if (value != null)
            {
                cached[key] = value;
            }
            else
            {
                Remove(key);
            }
        }

        public void Set<T>(string key, T value, int expires)
        {
            Set(key, value);
        }
    }
}
