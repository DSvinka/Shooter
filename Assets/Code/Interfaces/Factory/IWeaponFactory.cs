using Code.Data;
using Code.Models;

namespace Code.Interfaces.Factory
{
    internal interface IWeaponFactory
    {
        WeaponModel CreateWeapon(WeaponData weaponModel);
    }
}