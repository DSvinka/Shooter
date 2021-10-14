using System.Collections.Generic;
using Code.Controllers.ObjectPool;
using UnityEngine;

namespace Code.Services
{
    internal sealed class PoolService
    {
        private readonly Dictionary<string, ObjectPool> _cache;

        public PoolService()
        { 
            _cache = new Dictionary<string, ObjectPool>(32);
        }
        
        public GameObject Instantiate(GameObject prefab)
        {
            if (_cache.TryGetValue(prefab.name, out var viewPool)) 
                return viewPool.Pop();
            
            viewPool = new ObjectPool(prefab);
            _cache[prefab.name] = viewPool;
            return viewPool.Pop();
        }

        public void Destroy(GameObject value)
        {
            _cache[value.name].Push(value); 
        }
    }
}