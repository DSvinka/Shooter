using System;
using Code.Data.DataStores;
using Code.Interfaces.Data;
using Code.Interfaces.Factory;
using Code.Interfaces.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class EnemyFactory: IFactory
    {
        private DataStore _data;
        
        public EnemyFactory(DataStore data)
        {
            _data = data;
        }

        private (IEnemy enemy, GameObject gameObject) SetupEnemy(GameObject prefab, IEnemyData data)
        {
            var gameObject = Object.Instantiate(prefab);
            var enemy = gameObject.GetComponent<IEnemy>();
            if (enemy == null)
                throw new Exception($"IEnemy не найден в {gameObject.gameObject.name}");
            
            enemy.Health = data.MaxHealth;
            enemy.Armor = data.MaxArmor;
            enemy.Data = data;

            return (enemy, gameObject);
        }
        
        public IEnemy CreateTarget(Transform spawnPoint)
        {
            var (enemy, gameObject) = SetupEnemy(_data.TargetData.Prefab, _data.TargetData);
            gameObject.transform.SetParent(null);
            gameObject.transform.position = spawnPoint.position;
            gameObject.transform.rotation = spawnPoint.rotation;

            return enemy;
        }
        public IEnemyMelee CreateZombie(Transform spawnPoint)
        {
            var (enemy, gameObject) = SetupEnemy(_data.ZombieData.Prefab, _data.ZombieData);

            var enemyMelee = gameObject.GetComponent<IEnemyMelee>();
            if (enemy == null)
                throw new Exception($"IEnemyMelee не найден в {gameObject.gameObject.name}");
            
            gameObject.transform.SetParent(null);
            gameObject.transform.position = spawnPoint.position;
            gameObject.transform.rotation = spawnPoint.rotation;

            return enemyMelee;
        }
    }
}