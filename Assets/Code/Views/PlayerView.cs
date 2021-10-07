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

        public event Action<GameObject, int, float> OnDamage = delegate(GameObject attacker, int id, float damage) {  };
        public event Action<GameObject, int, float> OnArmored = delegate(GameObject armorer, int id, float armor) {  };
        public event Action<GameObject, int, float> OnHealing = delegate(GameObject healer, int id, float health) {  };

        public void AddHealth(GameObject healer, float health)
        {
            OnHealing.Invoke(healer, gameObject.GetInstanceID(), health);
        }

        public void AddArmor(GameObject armorer, float armor)
        {
            OnArmored.Invoke(armorer, gameObject.GetInstanceID(), armor);
        }

        public void AddDamage(GameObject attacker, float damage)
        {
            OnDamage.Invoke(attacker, gameObject.GetInstanceID(), damage);
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}