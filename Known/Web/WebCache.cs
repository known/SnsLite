using System;
using System.Web;

namespace Known.Web
{
    public class WebCache : ICache
    {
        public T Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default(T);

            return (T)HttpContext.Current.Cache.Get(key);
        }

        public void Remove(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public void RemoveAll()
        {
            var cache = HttpContext.Current.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }

        public void Set<T>(string key, T value)
        {
            Check.Required(key, "key");

            if (value != null)
            {
                HttpContext.Current.Cache.Insert(key, value);
            }
            else
            {
                Remove(key);
            }
        }

        public void Set<T>(string key, T value, int expires)
        {
            Check.Required(key, "key");

            if (value != null)
            {
                HttpContext.Current.Cache.Insert(key, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
            }
            else
            {
                Remove(key);
            }
        }
    }
}
