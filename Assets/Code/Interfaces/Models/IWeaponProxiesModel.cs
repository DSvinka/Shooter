using Code.Interfaces.Bridges;
using Code.Interfaces.Bridges.Weapon.Shoots;

namespace Code.Interfaces.Models
{
    internal interface IWeaponProxiesModel
    {
        IAim AimProxy { get; set; }
        IReload ReloadProxy { get; set; }
        
        IShoot ShootProxy { get; set; }
        ICast ShootCastProxy { get; set; }
        IDamage ShootDamageProxy { get; set; }

        void SetProxies(IAim aimProxy, IReload reloadProxy, IShoot shootProxy, ICast shootCastProxy, IDamage shootDamageProxy);
        void SetProxies(IWeaponProxiesModel weaponProxiesModel);
    }
}