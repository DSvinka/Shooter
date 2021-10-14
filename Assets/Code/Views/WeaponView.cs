using Code.Interfaces.Views;
using Code.Managers;
using UnityEngine;

namespace Code.Views
{
    internal sealed class WeaponView : MonoBehaviour, IWeaponView
    {
        [SerializeField] private GameObject _model;

        [SerializeField] private Transform _barrelPosition;
        [SerializeField] private Transform _aimPosition;
        
        [SerializeField] private WeaponManager.WeaponType _weaponType;

        public WeaponManager.WeaponType WeaponType => _weaponType;
        public GameObject Model => _model;

        public Transform BarrelPosition => _barrelPosition;
        public Transform AimPosition => _aimPosition;
    }
}