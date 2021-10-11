using Code.Models;

namespace Code.Decorators
{
    internal interface IWeaponModification
    {
        WeaponModel AddModification(WeaponModel weapon);
        WeaponModel RemoveModification(WeaponModel weapon);
        void ApplyModification(WeaponModel weapon);
    }
}