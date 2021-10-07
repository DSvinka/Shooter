using System;
using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IUnitView
    {
        public event Action<GameObject, IUnitView, float> OnDamage;
        public event Action<GameObject, IUnitView, float> OnArmored;
        public event Action<GameObject, IUnitView, float> OnHealing;

        public void AddHealth(GameObject healer, float health);
        public void AddArmor(GameObject armorer, float armor);
        public void AddDamage(GameObject attacker, float damage);
    }
}