using System;
using System.Collections.Generic;
using Code.Data.DataStores;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Units;
using Code.Managers;
using Code.Markers;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class EnemyInitialization: IInitialization
    {
        private readonly DataStore _data;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemySpawnMarker[] _enemySpawnMarkers;
        private List<IEnemyMelee> _meleeEnemies;
        private List<IEnemy> _enemies;
        
        public EnemyInitialization(DataStore data, EnemyFactory enemyFactory, EnemySpawnMarker[] enemySpawnMarkers)
        {
            _data = data;
            _enemyFactory = enemyFactory;
            _enemySpawnMarkers = enemySpawnMarkers;

            _enemies = new List<IEnemy>();
            _meleeEnemies = new List<IEnemyMelee>();
        }

        public void Initialization()
        {
            if (_enemySpawnMarkers.Length == 0)
                return;
            
            foreach (var enemySpawnMarker in _enemySpawnMarkers)
            {
                IEnemy enemy;

                switch (enemySpawnMarker.EnemyType)
                {
                    case EnemyManager.EnemyType.Target:
                        (enemy, _) = _enemyFactory.CreateTarget(enemySpawnMarker.transform);
                        break;
                
                    case EnemyManager.EnemyType.Zombie:
                        var (enemyMelee, _) = _enemyFactory.CreateZombie(enemySpawnMarker.transform);
                        enemy = enemyMelee as IEnemy;
                        if (enemy == null)
                            throw new Exception("Префаб зомби не имеет IEnemy интерфейса");
                        break;
                
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                AddEnemy(enemy, enemy.Data.EnemyType);
            }
        }

        public void AddEnemy(IEnemy enemy, EnemyManager.EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyManager.EnemyType.Target:
                    break;
                
                case EnemyManager.EnemyType.Zombie:
                    var enemyMelee = enemy as IEnemyMelee;
                    if (enemyMelee == null)
                        throw new Exception("Zombie не имеет интерфейса IEnemyMelee");
                    _meleeEnemies.Add(enemyMelee);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _enemies.Add(enemy);
        }

        public List<IEnemy> GetEnemies()
        {
            return _enemies;
        }
        public List<IEnemyMelee> GetMeleeEnemies()
        {
            return _meleeEnemies;
        }
    }
}