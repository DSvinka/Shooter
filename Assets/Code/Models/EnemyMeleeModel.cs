using Code.Interfaces.Data;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Models
{
    public class EnemyMeleeModel: IEnemyMeleeModel
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        public float Cooldown { get; set; }
        
        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }
        public NavMeshAgent NavMeshAgent { get; }
        public AudioSource AudioSource { get; }

        public IEnemyMeleeData Data { get; set; }
        public IEnemyMeleeView View { get; set; }

        public EnemyMeleeModel(IEnemyMeleeView view, GameObject viewGO, NavMeshAgent navMeshAgent, AudioSource audioSource, IEnemyMeleeData data)
        {
            View = view;
            Data = data;

            Transform = viewGO.transform;
            GameObject = viewGO;

            AudioSource = audioSource;
            NavMeshAgent = navMeshAgent;
            
            Health = data.MaxHealth;
            Armor = data.MaxArmor;
        }
    }
}