using System;
using Code.Interfaces.Views;
using UnityEngine;

namespace Code.Views
{
    internal sealed class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private Transform _aimPoint;
        [SerializeField] private Transform _weaponPoint;
        
        public Transform AimPoint => _aimPoint;
        public Transform WeaponPoint => _weaponPoint;

        public event Action<GameObject, IUnitView, float> OnDamage = delegate(GameObject attacker, IUnitView player, float damage) {  };
        public event Action<GameObject, IUnitView, float> OnArmored = delegate(GameObject armorer, IUnitView player, float armor) {  };
        public event Action<GameObject, IUnitView, float> OnHealing = delegate(GameObject healer, IUnitView player, float health) {  };

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