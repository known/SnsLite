﻿using System;
using System.Collections;

namespace Known
{
    public sealed class Container
    {
        private static Hashtable cached = new Hashtable();

        public static void Register<T, TImpl>() where TImpl : T
        {
            var key = typeof(T);
            if (!cached.ContainsKey(key))
            {
                lock (cached.SyncRoot)
                {
                    cached[key] = Activator.CreateInstance<TImpl>();
                }
            }
        }

        public static void Register<T, TImpl>(TImpl instance) where TImpl : T
        {
            var key = typeof(T);
            if (!cached.ContainsKey(key))
            {
                lock (cached.SyncRoot)
                {
                    cached[key] = instance;
                }
            }
        }

        public static T Load<T>()
        {
            var key = typeof(T);
            if (cached.ContainsKey(key))
            {
                return (T)cached[key];
            }

            return default(T);
        }
    }
}
