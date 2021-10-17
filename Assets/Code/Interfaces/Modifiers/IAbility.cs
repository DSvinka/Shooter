using Code.Modifiers.WeaponAbility;

namespace Code.Interfaces.Modifiers
{
    internal interface IAbility
    {
        string Name { get; }
        int Damage { get; }
        float DamageZoneSize { get; }
        
        ShootingType ShootingType { get; }
        DamageType DamageType { get; }
        CastType CastType { get; }
    }
}
