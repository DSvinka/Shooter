using Code.Data;
using Code.Interfaces.Bridges;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Interfaces.Models
{
    internal interface IWeaponModel
    {
        int BulletsLeft { get; set; }
        float FireCooldown { get; set; }

        bool IsReloading { get; set; }
        bool IsAiming { get; set; }
        public bool Blocking { get; set; }

        Transform BarrelPosition { get; }
        Transform AimPosition { get; }
        
        WeaponProxiesModel DefaultProxies { get; }
        WeaponProxiesModel Proxies { get; }

        ParticleSystem ParticleSystem { get; set; }
        AudioSource AudioSource { get; set; }
        AudioClip FireClip { get; }
        Transform Transform { get; }

        WeaponData Data { get; }
        WeaponView View { get; }

        void SetBarrelPosition(Transform position);
        void SetAimPosition(Transform position);
        void SetAudioClip(AudioClip audioClip);

        void ResetAllProxy();
        void SetDefaultProxy(WeaponProxiesModel weaponProxiesModel);
        
        void SetReloadProxy(IReload reload);
        void SetShootProxy(IShoot shoot);
        void SetAimProxy(IAim aim);
    }
}