using Code.Data;
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
        
        public ParticleSystem ParticleSystem { get; set; }
        public AudioSource AudioSource { get; set; }
        public Transform Transform { get; }
        
        public WeaponData Data { get; }
        public WeaponView View { get; }

        public WeaponModel(WeaponView view, WeaponData data, ParticleSystem particleSystem, AudioSource audioSource)
        {
            View = view;
            Data = data;
            
            Transform = View.transform;
            
            BulletsLeft = data.MagazineSize;
            ParticleSystem = particleSystem;
            AudioSource = audioSource;
        }
    }
}