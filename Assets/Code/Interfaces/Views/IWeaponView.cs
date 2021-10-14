using Code.Managers;
using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IWeaponView
    {
        WeaponManager.WeaponType WeaponType { get; }
        GameObject Model { get; }
        
        Transform BarrelPosition { get; }
        Transform AimPosition { get; }
    }
}