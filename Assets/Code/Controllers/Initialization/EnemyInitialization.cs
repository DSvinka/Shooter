using System;
using System.Collections.Generic;
using Code.Data.DataStores;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Managers;
using Code.Markers;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class EnemyInitialization: IInitialization
    {
        private readonly DataStore _data;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemySpawnMarker[] _enemySpawnMarkers;
        
        private List<IEnemyMeleeModel> _meleeEnemies;
        private List<IEnemyModel> _enemies;
        
        public EnemyInitialization(DataStore data, EnemyFactory enemyFactory, EnemySpawnMarker[] enemySpawnMarkers)
        {
            _data = data;
            _enemyFactory = enemyFactory;
            _enemySpawnMarkers = enemySpawnMarkers;

            _meleeEnemies = new List<IEnemyMeleeModel>();
            _enemies = new List<IEnemyModel>();
        }

        public void Initialization()
        {
            if (_enemySpawnMarkers.Length == 0)
                return;
            
            foreach (var enemySpawnMarker in _enemySpawnMarkers)
            {
                AddEnemy(enemySpawnMarker.transform, enemySpawnMarker.EnemyType);
            }
        }

        private void AddEnemy(Transform spawnPoint, EnemyManager.EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyManager.EnemyType.Target:
                    var enemyTarget = _enemyFactory.CreateEnemy(_data.TargetData, _data.TargetData.Prefab, spawnPoint);
                    _enemies.Add(enemyTarget);
                    break;
                
                case EnemyManager.EnemyType.Zombie:
                    var enemyZombie = _enemyFactory.CreateMeleeEnemy(_data.ZombieData, _data.ZombieData.Prefab, spawnPoint);
                    _meleeEnemies.Add(enemyZombie);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<IEnemyModel> GetEnemies()
        {
            return _enemies;
        }
        public List<IEnemyMeleeModel> GetMeleeEnemies()
        {
            return _meleeEnemies;
        }
    }
}