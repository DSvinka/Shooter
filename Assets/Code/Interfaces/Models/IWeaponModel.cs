using Code.Data;
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
        
        ParticleSystem ParticleSystem { get; set; }
        AudioSource AudioSource { get; set; }
        Transform Transform { get; }

        WeaponData Data { get; }
        WeaponView View { get; }
    }
}