using Code.Data;
using Code.Models;
using Code.Views;

namespace Code.Interfaces.Factory
{
    internal interface IWeaponFactory
    {
        WeaponModel CreateWeapon(WeaponView view, WeaponData data);
        WeaponView CreateWeapon(WeaponData weaponModel);
    }
}