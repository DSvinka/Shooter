using Code.Interfaces.Data;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;

namespace Code.Models
{
    internal sealed class EnemyModel: IEnemyModel
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        
        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }

        public IEnemyView View { get; }
        public IEnemyData Data { get; }

        public EnemyModel(IEnemyView view, GameObject viewGO, IEnemyData data)
        {
            View = view;
            Data = data;
            
            Transform = viewGO.transform;
            GameObject = viewGO;
            
            Health = data.MaxHealth;
            Armor = data.MaxArmor;
        }
    }
}