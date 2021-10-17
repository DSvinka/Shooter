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
        private readonly UnitStore _unitStore;
        private readonly EnemyFactory _enemyFactory;
        
        public ZombieFactory(UnitStore unitStore, EnemyFactory enemyFactory)
        {
            _unitStore = unitStore;
            _enemyFactory = enemyFactory;
        }

        public IEnemyModel CreateZombie(Vector3 position, Vector3 rotation)
        {
            var zombieData = _unitStore.ZombieData;
            return _enemyFactory.CreateEnemy(zombieData, zombieData.Prefab, new WalkMove(), new MeleeAttack(), position, rotation);
        }
    }
}