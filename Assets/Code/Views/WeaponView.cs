using Code.Managers;
using UnityEngine;

namespace Code.Views
{
    internal sealed class WeaponView : MonoBehaviour
    {
        [SerializeField] private GameObject _model;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private WeaponManager _weaponType;

        public WeaponManager WeaponType => _weaponType;
        public GameObject Model => _model;
        public Transform ShotPoint => _shotPoint;
    }
}