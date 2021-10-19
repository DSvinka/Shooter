using Code.Interfaces.Views;
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
            unit.OnDamage += OnUnitDamage;
            unit.OnArmored += OnUnitArmored;
            unit.OnHealing += OnUnitHealing;
        }

        private void OnUnitDamage(GameObject attacker, int id, float damage)
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
}