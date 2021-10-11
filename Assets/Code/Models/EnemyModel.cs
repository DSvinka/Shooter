using System;
using Code.Interfaces.Bridges;
using Code.Interfaces.Data;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Models
{
    [Serializable]
    internal sealed class EnemyModel: IEnemyModel
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        public float AttackCooldown { get; set; }
        
        public Transform SpawnPoint { get; set; }
        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }

        public IMove MoveBridge { get; private set; }
        public IAttack AttackBridge { get; private set; }
        
        public NavMeshAgent NavMeshAgent { get; private set;  }

        public AudioSource AudioSource { get; private set; }

        public IEnemyView View { get; }
        public IEnemyData Data { get; }

        public void Reset()
        {
            Health = Data.MaxHealth;
            Armor = Data.MaxArmor;
        }

        public EnemyModel(IEnemyView view, GameObject viewGO, IEnemyData data)
        {
            View = view;
            Data = data;
            
            Transform = viewGO.transform;
            GameObject = viewGO;

            Health = data.MaxHealth;
            Armor = data.MaxArmor;
        }

        public void SetComponents(NavMeshAgent navMeshAgent, AudioSource audioSource)
        {
            NavMeshAgent = navMeshAgent;
            AudioSource = audioSource;
        }
        
        public void SetBridges(IMove moveBridge, IAttack attackBridge)
        {
            MoveBridge = moveBridge;
            AttackBridge = attackBridge;
        }
    }
}