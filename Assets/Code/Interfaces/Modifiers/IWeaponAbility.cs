using System.Collections;
using System.Collections.Generic;
using Code.Modifiers.WeaponAbility;

namespace Code.Interfaces.Modifiers
{
    internal interface IWeaponAbility
    {
        IAbility this[int index] { get; }
        string this[ShootingType index] { get; }
        
        int MaxDamage { get; }
        
        IEnumerable<IAbility> GetAbility();
        IEnumerator GetEnumerator();
        IEnumerable<IAbility> GetAbility(DamageType index);
    }
}
