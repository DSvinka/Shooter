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
        public bool Blocking { get; set; }
        
        public Transform BarrelPosition { get; private set; }
        public Transform AimPosition { get; private set; }
        
        public WeaponProxiesModel DefaultProxies { get; private set; }
        public WeaponProxiesModel Proxies { get; }

        public ParticleSystem ParticleSystem { get; set; }
        public AudioSource AudioSource { get; set; }
        public AudioClip FireClip { get; private set; }
        
        public Transform Transform { get; }
        public GameObject GameObject { get; }

        public WeaponData Data { get; }
        public WeaponView View { get; }

        public WeaponModel(WeaponView view, WeaponData data, ParticleSystem particleSystem, AudioSource audioSource)
        {
            View = view;
            Data = data;

            Blocking = false;
            Transform = View.transform;
            GameObject = View.gameObject;

            Proxies = new WeaponProxiesModel();
            DefaultProxies = new WeaponProxiesModel();
            
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
        
        public void ResetBarrelPosition()
        {
            SetBarrelPosition(View.BarrelPosition);
        }

        public void SetAudioClip(AudioClip audioClip)
        {
            FireClip = audioClip;
        }
        
        public void ResetAudioClip()
        {
            SetAudioClip(Data.FireClip);
        }
        
        public void SetDefaultProxy(WeaponProxiesModel weaponProxiesModel)
        {
            DefaultProxies.SetProxies(weaponProxiesModel);
        }

        public void ResetAllProxy()
        {
            Proxies.SetProxies(DefaultProxies);
        }

        public void SetReloadProxy(IReload reload)
        {
            Proxies.ReloadProxy = reload;
        }
        
        public void SetShootProxy(IShoot shoot)
        {
            Proxies.ShootProxy = shoot;
        }

        public void SetAimProxy(IAim aim)
        {
            Proxies.AimProxy = aim;
        }
    }
}