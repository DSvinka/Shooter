using System;
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

        public IEnemyModel CreateEnemy(IEnemyData data, GameObject prefab, IMove moveBridge, IAttack attackBridge, Vector3 position, Vector3 rotation)
        {
            var gameObject = Object.Instantiate(prefab, null, true);
            if (!gameObject.TryGetComponent(out IEnemyView view))
                throw new Exception($"IEnemyMeleeView не найден в {gameObject.gameObject.name}!");
            
            if (!gameObject.TryGetComponent(out NavMeshAgent navMeshAgent))
                throw new Exception($"NavMeshAgent не найден в {gameObject.gameObject.name}!");
            
            if (!gameObject.TryGetComponent(out AudioSource audioSource))
                throw new Exception($"AudioSource не найден в {gameObject.gameObject.name}!");

            var enemyModel = new EnemyModel(view, gameObject, data)
            {
                SpawnPointPosition = position,
                SpawnPointRotation = rotation
            };
            enemyModel.SetComponents(navMeshAgent, audioSource);
            enemyModel.SetBridges(moveBridge, attackBridge);
            
            audioSource.pitch = Random.Range(data.MinRandomSoundPitch, data.MaxRandomSoundPitch);

            gameObject.transform.position = position;
            gameObject.transform.eulerAngles = rotation;

            return enemyModel;
        }
    }
}