using Code.Interfaces.Bridges;
using Code.Interfaces.Models;

namespace Code.Models
{
    internal sealed class WeaponProxiesModel: IWeaponProxiesModel
    {
        public IReload ReloadProxy { get; set; }
        public IShoot ShootProxy { get; set; }
        public IAim AimProxy { get; set; }

        public void SetProxies(IReload reloadProxy, IShoot shootProxy, IAim aimProxy)
        {
            ReloadProxy = reloadProxy;
            ShootProxy = shootProxy;
            AimProxy = aimProxy;
        }

        public void SetProxies(IWeaponProxiesModel weaponProxiesModel)
        {
            ReloadProxy = weaponProxiesModel.ReloadProxy;
            ShootProxy = weaponProxiesModel.ShootProxy;
            AimProxy = weaponProxiesModel.AimProxy;
        }
    }
}