using Code.Models;

namespace Code.Decorators
{
    internal interface IWeaponModification
    {
        WeaponModel AddModification(WeaponModel weapon);
        void RemoveModification();
        void ApplyModification(WeaponModel weapon);
    }
}