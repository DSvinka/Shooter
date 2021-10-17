using Code.Interfaces.Bridges;
using Code.Interfaces.Bridges.Weapon.Shoots;
using Code.Interfaces.Models;
using Code.Modifiers.WeaponAbility;

namespace Code.Models
{
    internal sealed class WeaponProxiesModel: IWeaponProxiesModel
    {
        public IAim AimProxy { get; set; }
        public IReload ReloadProxy { get; set; }
        
        public IShoot ShootProxy { get; set; }
        public ICast ShootCastProxy { get; set; }
        public IDamage ShootDamageProxy { get; set; }

        public void SetProxies(IAim aimProxy, IReload reloadProxy, IShoot shootProxy, ICast shootCastProxy, IDamage shootDamageProxy)
        {
            AimProxy = aimProxy;
            ReloadProxy = reloadProxy;
            
            ShootProxy = shootProxy;
            ShootCastProxy = shootCastProxy;
            ShootDamageProxy = shootDamageProxy;
        }

        public void SetProxies(IWeaponProxiesModel weaponProxiesModel)
        {
            AimProxy = weaponProxiesModel.AimProxy;
            ReloadProxy = weaponProxiesModel.ReloadProxy;
            
            ShootProxy = weaponProxiesModel.ShootProxy;
            ShootCastProxy = weaponProxiesModel.ShootCastProxy;
            ShootDamageProxy = weaponProxiesModel.ShootDamageProxy;
        }
    }
}