using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data
{
    // TODO: Может разделить пулю от оружия на 2 класса?
    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapons/Weapon")]
    public sealed class WeaponData : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _weaponPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _bulletPrefabPath;

        [Header("Характеристики Оружия")]
        [SerializeField] private int _damage = 10;
        
        [SerializeField] [Tooltip("Максимальное количество патрон в обойме")]
        private int _magazineSize = 32;
        
        [SerializeField] [Tooltip("Скорострельность оружия (Промежуток между выстрелами в секундах)")] [Range(0.1f, 20f)]
        private float _fireRate = 0.1f;

        [SerializeField] [Tooltip("Максимальное расстояние на которое может стрелять оружие")]
        private float _maxDistance = 3f;

        [SerializeField] [Tooltip("Размер разброса пуль")]
        private float _spread = 10f;
        
        [SerializeField] [Tooltip("Размер разброса пуль при прицеливании")]
        private float _spreadAim = 5f;
        
        [Header("Характеристики Пуль")]
        [SerializeField] [Tooltip("Скорость с которой будет лететь пуля")]
        private float _bulletForce = 50f;
        
        [SerializeField] [Tooltip("Сколько времени пуля будет жить перед тем как исчезнет (в секундах)")]
        private float _bulletLifeTime = 3f;

        [Header("Аудио")]
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _reloadClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _fireClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _noAmmoClipPath;
        
        [Header("Визуал")]
        [SerializeField] private Vector3 _reloadMove;
        
        [SerializeField] private LayerMask _layerMask;

        #endregion
        
        #region Объекты
        
        private GameObject _weaponPrefab;
        private GameObject _bulletPrefab;
        
        private AudioClip _reloadClip;
        private AudioClip _fireClip;
        private AudioClip _noAmmoClip;
        
        #endregion
        
        #region Свойства

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
        public LayerMask LayerMask => _layerMask;

        public GameObject WeaponPrefab => GetData(_weaponPrefabPath, _weaponPrefab);
        public GameObject BulletPrefab => GetData(_bulletPrefabPath, _bulletPrefab);
        
        public AudioClip ReloadClip => GetData(_reloadClipPath, _reloadClip);
        public AudioClip FireClip => GetData(_fireClipPath, _fireClip);
        public AudioClip NoAmmoClip => GetData(_noAmmoClipPath, _noAmmoClip);

        #endregion
    }
}