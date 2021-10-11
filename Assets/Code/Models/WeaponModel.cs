using Code.Data;
using Code.Interfaces.Bridges;
using Code.Interfaces.Models;
using Code.Views;
using UnityEngine;

namespace Code.Models
{
    internal sealed class WeaponModel: IWeaponModel
    {
        public int BulletsLeft { get; set; }
        public float FireCooldown { get; set; }
        
        public bool IsReloading { get; set; }
        public bool IsAiming { get; set; }
        
        public Transform BarrelPosition { get; private set; }
        public Transform AimPosition { get; private set; }
        
        public IReload ReloadDefaultProxy { get; private set; }
        public IShoot ShootDefaultProxy { get; private set; }
        public IAim AimDefaultProxy { get; private set; }
        
        public IReload ReloadProxy { get; private set; }
        public IShoot ShootProxy { get; private set; }
        public IAim AimProxy { get; private set; }

        public ParticleSystem ParticleSystem { get; set; }
        public AudioSource AudioSource { get; set; }
        public AudioClip FireClip { get; private set; }
        public Transform Transform { get; }
        
        public WeaponData Data { get; }
        public WeaponView View { get; }

        public WeaponModel(WeaponView view, WeaponData data, ParticleSystem particleSystem, AudioSource audioSource)
        {
            View = view;
            Data = data;
            
            Transform = View.transform;
            
            SetBarrelPosition(view.BarrelPosition);
            SetAimPosition(view.AimPosition);
            SetAudioClip(data.FireClip);
            
            BulletsLeft = data.MagazineSize;
            ParticleSystem = particleSystem;
            AudioSource = audioSource;
        }

        public void SetBarrelPosition(Transform position)
        {
            BarrelPosition = position;
        }

        public void SetAimPosition(Transform position)
        {
            AimPosition = position;
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            FireClip = audioClip;
        }
        
        public void ResetAllProxy()
        {
            ReloadProxy = ReloadDefaultProxy;
            ShootProxy = ShootDefaultProxy;
            AimProxy = AimDefaultProxy;
        }

        public void SetDefaultProxy(IReload reload, IShoot shoot, IAim aim)
        {
            ReloadDefaultProxy = reload;
            ShootDefaultProxy = shoot;
            AimDefaultProxy = aim;
        }

        public void SetReloadProxy(IReload reload)
        {
            ReloadProxy = reload;
        }
        
        public void SetShootProxy(IShoot shoot)
        {
            ShootProxy = shoot;
        }

        public void SetAimProxy(IAim aim)
        {
            AimProxy = aim;
        }
    }
}