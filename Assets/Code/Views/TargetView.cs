using System;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;

namespace Code.Views
{
    internal sealed class TargetView: MonoBehaviour, IEnemyView
    {
        public IEnemyModel Model { get; set; }
        
        public event Action<GameObject, IUnitView, float> OnDamage = delegate(GameObject attacker, IUnitView target, float damage) {  };
        public event Action<GameObject, IUnitView, float> OnArmored = delegate(GameObject armorer, IUnitView target, float armor) {  };
        public event Action<GameObject, IUnitView, float> OnHealing = delegate(GameObject healer, IUnitView target, float health) {  };

        public void AddHealth(GameObject healer, float health)
        {
            OnHealing.Invoke(healer, this, health);
        }

        public void AddArmor(GameObject armorer, float armor)
        {
            OnArmored.Invoke(armorer, this, armor);
        }

        public void AddDamage(GameObject attacker, float damage)
        {
            OnDamage.Invoke(attacker, this, damage);
        }
    }
}