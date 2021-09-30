using System;
using Code.Interfaces.Data;
using Code.Interfaces.Units;
using UnityEngine;

namespace Code.Views
{
    internal sealed class TargetView: MonoBehaviour, IEnemy
    {
        public float Health { get; set; }
        public float Armor { get; set; }
        public IUnitData Data { get; set; }

        public event Action<GameObject, IUnit, float> OnDamage = delegate(GameObject attacker, IUnit target, float damage) {  };
        public event Action<GameObject, IUnit, float> OnArmored = delegate(GameObject armorer, IUnit target, float armor) {  };
        public event Action<GameObject, IUnit, float> OnHealing = delegate(GameObject healer, IUnit target, float health) {  };

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