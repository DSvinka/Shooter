using System;
using UnityEngine;

namespace Code.Interfaces.Units
{
    public interface IUnit
    {
        event Action<GameObject, IUnit, float> OnDamage;
        event Action<GameObject, IUnit, float> OnArmored;
        event Action<GameObject, IUnit, float> OnHealing;

        void AddHealth(GameObject healer, float health);
        void AddArmor(GameObject armorer, float armor);
        void AddDamage(GameObject attacker, float damage);
    }
}