using System;
using Code.Interfaces.Views;
using Code.Utils.Extensions;
using UnityEngine;

namespace Code.Listeners
{
    internal sealed class UnitLogListener
    {
        public void Add(IUnitView unit)
        {
            unit.OnDamage += OnUnitDamage;
            unit.OnArmored += OnUnitArmored;
            unit.OnHealing += OnUnitHealing;
        }

        public void Remove(IUnitView unit)
        {
            unit.OnDamage -= OnUnitDamage;
            unit.OnArmored -= OnUnitArmored;
            unit.OnHealing -= OnUnitHealing;
        }

        private void OnUnitDamage(GameObject attacker, Vector3 damagePosition, int id, float damage)
        {
            Debug.Log($"\"{attacker.name}\" [{attacker.GetInstanceID()}] нанёс {damage} урона юниту с id = {id}");
        }
        private void OnUnitArmored(GameObject armored, int id, float armor)
        {
            Debug.Log($"\"{armored.name}\" [{armored.GetInstanceID()}] нанёс {armor} урона юниту с id = {id}");
        }
        private void OnUnitHealing(GameObject healer, int id, float health)
        {
            Debug.Log($"\"{healer.name}\" [{healer.GetInstanceID()}] нанёс {health} урона юниту с id = {id}");
        }
    }
    
    // TODO: Добавить отдельные интерфейсы для дамага, лечения, бронирования
    internal sealed class UnitListener
    {
        public void Add(IUnitView unit)
        {
            unit.OnDamage += OnDamage;
            unit.OnArmored += OnArmored;
            unit.OnHealing += OnHealing;
        }

        public void Remove(IUnitView unit)
        {
            unit.OnDamage -= OnDamage;
            unit.OnArmored -= OnArmored;
            unit.OnHealing -= OnHealing;
        }

        public event Action<GameObject, Vector3, int, float> OnUnitDamage = delegate(GameObject attacker, Vector3 damagePosition, int id, float damage) {  };
        public event Action<GameObject, int, float> OnUnitArmored = delegate(GameObject armorer, int id, float armor) {  };
        public event Action<GameObject, int, float> OnUnitHealing = delegate(GameObject healer, int id, float health) {  };

        private void OnDamage(GameObject attacker, Vector3 damagePosition, int id, float damage)
        {
            OnUnitDamage.Invoke(attacker, damagePosition, id, damage);
        }
        private void OnArmored(GameObject armored, int id, float armor)
        {
            OnUnitArmored.Invoke(armored, id, armor);
        }
        private void OnHealing(GameObject healer, int id, float health)
        {
            OnUnitHealing.Invoke(healer, id, health);
        }
    }
}