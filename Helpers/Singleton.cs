using System;
using System.Collections.Concurrent;

namespace do9Rename.Helpers
{
    internal static class Singleton<T> where T : new()
    {
        private static ConcurrentDictionary<Type, T> Instances
            => new ConcurrentDictionary<Type, T>();

        public static T Instance => Instances.GetOrAdd(typeof(T), new T());
    }
}
