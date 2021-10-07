using System;
using System.Collections.Generic;

namespace Code.Services
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _serviceСontainer = new Dictionary<Type, object>();

        public static void SetService<T>(T value) where T : class
        {
            var typeValue = typeof(T);
            if (!_serviceСontainer.ContainsKey(typeValue))
            {
                _serviceСontainer[typeValue] = value;
            }
        }
 
        public static T Resolve<T>()
        { var type = typeof(T);

            if (_serviceСontainer.ContainsKey(type))
            {
                return (T) _serviceСontainer[type];
            }

            return default;
        }
    }
}