using Code.Data.DataStores;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class EnemyFactory: IFactory
    {
        private DataStore _data;
        
        public EnemyFactory(DataStore data)
        {
            _data = data;
        }

        public Transform CreateTarget()
        {
            var target = Object.Instantiate(_data.TargetData.TargetPrefab);
            return target.transform;
        }
    }
}