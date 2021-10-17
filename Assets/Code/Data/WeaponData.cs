using Code.Data.WeaponModifications;
using static Code.Utils.Extensions.DataUtils;
using Code.Interfaces.Data;
using Code.Managers;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapons/Weapon")]
    internal sealed class WeaponData : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Пути
        
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _weaponPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _bulletPrefabPath;
        
        [Header("Модификации по умолчанию")]
        [SerializeField] [AssetPath.Attribute(typeof(AimModificatorData))] private string _defaultAimPath;
        [SerializeField] [AssetPath.Attribute(typeof(BarrelModificatorData))] private string _defaultBarrelPath;

        [Header("Модификации")]
        [SerializeField] [AssetPath.Attribute(typeof(AimModificatorData))] private string[] _aimsPaths;
        [SerializeField] [AssetPath.Attribute(typeof(BarrelModificatorData))] private string[] _barrelsPaths;
        
        [Header("Аудио Клипы")]
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _reloadClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _fireClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _noAmmoClipPath;
        
        #endregion

        #region Поля

        [Header("Характеристики Оружия")]
        [SerializeField] private int _damage = 10;
        
        [SerializeField] [Tooltip("Максимальное количество патрон в обойме")]
        private int _magazineSize = 32;
        
        [SerializeField] [Tooltip("Скорострельность оружия (Промежуток между выстрелами в секундах)")] [Range(0.1f, 20f)]
        private float _fireRate = 0.1f;

        [SerializeField] [Tooltip("Максимальное расстояние на которое может стрелять оружие")]
        private float _maxDistance = 50f;

        [SerializeField] [Tooltip("Размер разброса пуль")]
        private float _spread = 10f;
        
        [SerializeField] [Tooltip("Размер разброса пуль при прицеливании")]
        private float _spreadAim = 5f;
        
        [SerializeField] [Tooltip("На каких объектах луч выстрела будет останавливаться")]
        private LayerMask _rayCastLayerMask;

        [SerializeField] private WeaponManager.WeaponType _weaponType;
        
        [Header("Характеристики Пуль")]
        [SerializeField] [Tooltip("Скорость с которой будет лететь пуля")]
        private float _bulletForce = 100f;
        
        [SerializeField] [Tooltip("Сколько времени пуля будет жить перед тем как исчезнет (в секундах)")]
        private float _bulletLifeTime = 3f;

        [Header("Визуал")]
        [SerializeField] private Vector3 _reloadMove;

        #endregion
        
        #region Объекты
        
        private GameObject _weaponPrefab;
        private GameObject _bulletPrefab;
        
        private AimModificatorData _defaultAim;
        private BarrelModificatorData _defaultBarrel;
        
        private AimModificatorData[] _aims;
        private BarrelModificatorData[] _barrels;
        
        private AudioClip _reloadClip;
        private AudioClip _fireClip;
        private AudioClip _noAmmoClip;
        
        #endregion
        
        #region Публичные Свойства

        // Характеристики Оружия
        public int Damage => _damage;
        public int MagazineSize => _magazineSize;
        public float FireRate => _fireRate;
        public float MaxDistance => _maxDistance;

        // Характеристики Пули
        public float BulletLifetime => _bulletLifeTime;
        public float BulletForce => _bulletForce;
        public float Spread => _spread;
        public float SpreadAim => _spreadAim;
        
        public Vector3 ReloadMove => _reloadMove;
        public LayerMask RayCastLayerMask => _rayCastLayerMask;
        public WeaponManager.WeaponType WeaponType => _weaponType;

        public GameObject WeaponPrefab => GetData(_weaponPrefabPath, _weaponPrefab);
        public GameObject BulletPrefab => GetData(_bulletPrefabPath, _bulletPrefab);
        
        public AimModificatorData DefaultAimModificator => GetData(_defaultAimPath, _defaultAim);
        public BarrelModificatorData DefaultBarrelModificator => GetData(_defaultBarrelPath, _defaultBarrel);
        
        public AimModificatorData[] AimModifications => GetDataList(_aimsPaths, _aims);
        public BarrelModificatorData[] BarrelModifications => GetDataList(_barrelsPaths, _barrels);
        
        public AudioClip ReloadClip => GetData(_reloadClipPath, _reloadClip);
        public AudioClip FireClip => GetData(_fireClipPath, _fireClip);
        public AudioClip NoAmmoClip => GetData(_noAmmoClipPath, _noAmmoClip);

        #endregion
    }
}