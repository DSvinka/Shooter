using System;
using System.Collections.Generic;
using Code.Bridges.Attacks;
using Code.Bridges.Movement;
using Code.Data.DataStores;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Managers;
using Code.Markers;
using Code.SaveData;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class EnemyInitialization: IInitialization
    {
        private readonly DataStore _data;
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemySpawnMarker[] _enemySpawnMarkers;
        
        private Dictionary<int, IEnemyModel> _enemies;
        
        public EnemyInitialization(DataStore data, EnemyFactory enemyFactory, EnemySpawnMarker[] enemySpawnMarkers)
        {
            _data = data;
            _enemyFactory = enemyFactory;
            _enemySpawnMarkers = enemySpawnMarkers;
            
            _enemies = new Dictionary<int, IEnemyModel>();
        }

        public void SetEnemies(Dictionary<int, IEnemyModel> enemies)
        {
            _enemies = enemies;
        }

        public void Initialization()
        {
            if (_enemySpawnMarkers.Length == 0)
                return;
            
            if (_enemies.Count != 0)
                return;
            
            foreach (var enemySpawnMarker in _enemySpawnMarkers)
            {
                AddEnemy(enemySpawnMarker.transform, enemySpawnMarker.EnemyType);
            }
        }

        private void AddEnemy(Transform spawnPoint, EnemyManager.EnemyType enemyType)
        {
            var enemy = enemyType switch
            {
                EnemyManager.EnemyType.Zombie => _enemyFactory.CreateEnemy(_data.ZombieData, _data.ZombieData.Prefab, new WalkMove(), new MeleeAttack(), spawnPoint.position, spawnPoint.rotation.eulerAngles),
                _ => throw new ArgumentOutOfRangeException()
            };
            _enemies.Add(enemy.GameObject.GetInstanceID(), enemy);
        }

        public Dictionary<int, IEnemyModel> GetEnemies()
        {
            return _enemies;
        }
    }
}