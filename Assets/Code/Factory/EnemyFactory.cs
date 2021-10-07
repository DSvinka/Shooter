using System;
using Code.Bridges.Attacks;
using Code.Bridges.Movement;
using Code.Data.DataStores;
using Code.Interfaces.Bridges;
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

        public IEnemyModel CreateEnemy(IEnemyData data, GameObject prefab, IMove moveBridge, IAttack attackBridge, Transform spawnPoint)
        {
            var gameObject = Object.Instantiate(prefab, null, true);
            var enemyView = gameObject.GetComponent<IEnemyView>();
            if (enemyView == null)
                throw new Exception($"IEnemyMeleeView не найден в {gameObject.gameObject.name}!");
            
            var enemyNavMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            if (enemyNavMeshAgent == null)
                throw new Exception($"NavMeshAgent не найден в {gameObject.gameObject.name}!");
            
            var enemyAudioSource = gameObject.GetComponent<AudioSource>();
            if (enemyAudioSource == null)
                throw new Exception($"AudioSource не найден в {gameObject.gameObject.name}!");

            var enemyModel = new EnemyModel(enemyView, gameObject, data)
            {
                SpawnPoint = spawnPoint,
            };
            enemyModel.SetComponents(enemyNavMeshAgent, enemyAudioSource);
            enemyModel.SetBridges(moveBridge, attackBridge);
            
            enemyAudioSource.pitch = Random.Range(data.MinRandomSoundPitch, data.MaxRandomSoundPitch);

            gameObject.transform.position = spawnPoint.position;
            gameObject.transform.rotation = spawnPoint.rotation;

            return enemyModel;
        }
    }
}