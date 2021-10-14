using Code.Interfaces.Bridges;

namespace Code.Interfaces.Models
{
    internal interface IWeaponProxiesModel
    {
        IReload ReloadProxy { get; set; }
        IShoot ShootProxy { get; set; }
        IAim AimProxy { get; set; }

        void SetProxies(IReload reloadProxy, IShoot shootProxy, IAim aimProxy);
        void SetProxies(IWeaponProxiesModel weaponProxiesModel);
    }
}