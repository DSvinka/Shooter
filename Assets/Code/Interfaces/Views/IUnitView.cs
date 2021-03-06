using System;
using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IUnitView
    {
        event Action<GameObject, Vector3, int, float> OnDamage;
        event Action<GameObject, int, float> OnArmored;
        event Action<GameObject, int, float> OnHealing;

        void AddHealth(GameObject healer, float health);
        void AddArmor(GameObject armorer, float armor);
        void AddDamage(GameObject attacker, Vector3 damagePosition, float damage);
    }
}