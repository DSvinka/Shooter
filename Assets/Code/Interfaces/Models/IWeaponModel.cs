using Code.Data;
using Code.Interfaces.Bridges;
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
        
        // TODO: Создать класс чтобы складировать прокси.
        IReload ReloadDefaultProxy { get; }
        IShoot ShootDefaultProxy { get; }
        IAim AimDefaultProxy { get; }
        
        IReload ReloadProxy { get; }
        IShoot ShootProxy { get; }
        IAim AimProxy { get; }

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
        void SetDefaultProxy(IReload reload, IShoot shoot, IAim aim);
        
        void SetReloadProxy(IReload reload);
        void SetShootProxy(IShoot shoot);
        void SetAimProxy(IAim aim);
    }
}