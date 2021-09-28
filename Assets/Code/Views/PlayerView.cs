using System;
using Code.Interfaces.Units;
using UnityEngine;

namespace Code.Views
{
    internal sealed class PlayerView : MonoBehaviour, IUnit
    {
        // TODO: Заменить этот костыль на нормальный подбор оружия
        [SerializeField] private WeaponView _weapon;
        public WeaponView Weapon => _weapon;
        
        public event Action<GameObject, IUnit, float> OnDamage = delegate(GameObject attacker, IUnit player, float damage) {  };
        public event Action<GameObject, IUnit, float> OnArmored = delegate(GameObject armorer, IUnit player, float armor) {  };
        public event Action<GameObject, IUnit, float> OnHealing = delegate(GameObject healer, IUnit player, float health) {  };

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

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}