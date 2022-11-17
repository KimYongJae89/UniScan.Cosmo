using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Standard.DynMvp.Base.Helpers
{
    public static class Singleton<T>
        where T : new()
    {
        private static ConcurrentDictionary<Type, T> _instances = new ConcurrentDictionary<Type, T>();

        public static T Instance
        {
            get
            {
                return _instances.GetOrAdd(typeof(T), (t) => new T());
            }
        }

        public static void SetInstance(T t)
        {
            _instances[typeof(T)] = t;
        }
    }
}
