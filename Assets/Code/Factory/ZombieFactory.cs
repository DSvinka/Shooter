using Code.Bridges.Attacks;
using Code.Bridges.Movement;
using Code.Data.DataStores;
using Code.Interfaces.Factory;
using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class ZombieFactory: IFactory, IZombieFactory
    {
        private readonly DataStore _data;
        private readonly EnemyFactory _enemyFactory;
        
        public ZombieFactory(DataStore data, EnemyFactory enemyFactory)
        {
            _data = data;
            _enemyFactory = enemyFactory;
        }

        public IEnemyModel CreateZombie(Vector3 position, Vector3 rotation)
        {
            var zombieData = _data.ZombieData;
            return _enemyFactory.CreateEnemy(zombieData, zombieData.Prefab, new WalkMove(), new MeleeAttack(), position, rotation);
        }
    }
}