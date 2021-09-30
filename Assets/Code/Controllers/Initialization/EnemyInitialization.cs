using System;
using System.Collections.Generic;
using Code.Data.DataStores;
using Code.Factory;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Units;
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
        private List<IEnemy> _enemies;

        public EnemyInitialization(DataStore data, EnemyFactory enemyFactory, EnemySpawnMarker[] enemySpawnMarkers)
        {
            _data = data;
            _enemyFactory = enemyFactory;
            _enemySpawnMarkers = enemySpawnMarkers;

            _enemies = new List<IEnemy>();
        }

        public void Initialization()
        {
            if (_enemySpawnMarkers.Length == 0)
                return;

            Transform enemy;
            Transform spawnerTransform;
            foreach (var enemySpawnMarker in _enemySpawnMarkers)
            {
                IEnemy enemyComponent;
                switch (enemySpawnMarker.EnemyType)
                {
                    case Enemies.Target:
                        enemy = _enemyFactory.CreateTarget();
                        enemyComponent = enemy.GetComponent<IEnemy>();
                        if (enemyComponent == null)
                            throw new Exception($"IEnemy не найден в {enemy.gameObject.name}");
                        enemyComponent.Health = _data.TargetData.MaxHealth;
                        enemyComponent.Armor = _data.TargetData.MaxArmor;
                        enemyComponent.Data = _data.TargetData;
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                spawnerTransform = enemySpawnMarker.transform;
                enemy.SetParent(null);
                enemy.position = spawnerTransform.position;
                enemy.rotation = spawnerTransform.rotation;

                _enemies.Add(enemyComponent);
            }
        }

        public List<IEnemy> GetEnemies()
        {
            return _enemies;
        }
    }
}