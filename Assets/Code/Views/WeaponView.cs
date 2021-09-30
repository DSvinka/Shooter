using Code.Data;
using UnityEngine;

namespace Code.Views
{
    internal sealed class WeaponView : MonoBehaviour
    {
        [Header("Модельки и расположения")]
        [SerializeField] private GameObject _model;
        [SerializeField] private Transform _shotPoint;

        [Header("Характеристики Оружия")]
        [SerializeField] private WeaponData _weaponData; // TODO: Заменить это на ENUM

        public WeaponData WeaponData => _weaponData;
        public GameObject Model => _model;
        public Transform ShotPoint => _shotPoint;
    }
}