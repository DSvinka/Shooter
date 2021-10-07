using System;
using Code.Data.DataStores;
using Code.Interfaces.Data;
using Code.Interfaces.Factory;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using Code.Models;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Code.Factory
{
    internal sealed class EnemyFactory: IFactory, IEnemyFactory
    {
        private DataStore _data;
        
        public EnemyFactory(DataStore data)
        {
            _data = data;
        }

        private (IEnemyView enemy, GameObject gameObject) SetupEnemy(GameObject prefab, IEnemyData data)
        {
            var gameObject = Object.Instantiate(prefab);
            var enemy = gameObject.GetComponent<IEnemyView>();
            if (enemy == null)
                throw new Exception($"IEnemyView не найден в {gameObject.gameObject.name}");

            return (enemy, gameObject);
        }
        
        public IEnemyModel CreateEnemy(IEnemyData data, GameObject prefab, Transform spawnPoint)
        {
            var gameObject = Object.Instantiate(prefab, null, true);
            var enemyView = gameObject.GetComponent<IEnemyView>();
            if (enemyView == null)
                throw new Exception($"IEnemyView не найден в {gameObject.gameObject.name}");

            var enemyModel = new EnemyModel(enemyView, gameObject, data)
            {
                SpawnPoint = spawnPoint,
                Health = data.MaxHealth,
                Armor = data.MaxArmor
            };
            enemyView.Model = enemyModel;

            gameObject.transform.position = spawnPoint.position;
            gameObject.transform.rotation = spawnPoint.rotation;

            return enemyModel;
        }
        public IEnemyMeleeModel CreateMeleeEnemy(IEnemyMeleeData data, GameObject prefab, Transform spawnPoint)
        {
            var gameObject = Object.Instantiate(prefab, null, true);
            var enemyMeleeView = gameObject.GetComponent<IEnemyMeleeView>();
            if (enemyMeleeView == null)
                throw new Exception($"IEnemyMeleeView не найден в {gameObject.gameObject.name}!");
            
            var enemyNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            if (enemyNavMeshAgent == null)
                throw new Exception($"NavMeshAgent не найден в {gameObject.gameObject.name}!");
            
            var enemyAudioSource = gameObject.GetComponent<AudioSource>();
            if (enemyAudioSource == null)
                throw new Exception($"AudioSource не найден в {gameObject.gameObject.name}!");

            var enemyMeleeModel = new EnemyMeleeModel(enemyMeleeView, gameObject, enemyNavMeshAgent, enemyAudioSource, data)
            {
                SpawnPoint = spawnPoint,
                Health = data.MaxHealth,
                Armor = data.MaxArmor,
                Pitch = Random.Range(data.MinPitch, data.MaxPitch)
            };
            enemyMeleeView.Model = enemyMeleeModel;
            enemyAudioSource.pitch = enemyMeleeModel.Pitch;

            gameObject.transform.position = spawnPoint.position;
            gameObject.transform.rotation = spawnPoint.rotation;

            return enemyMeleeModel;
        }
    }
}