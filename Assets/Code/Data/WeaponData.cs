using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data
{
    public enum WeaponSlotType
    {
        Small,
        Middle
    }

    [CreateAssetMenu(fileName = "Weapon", menuName = "Data/Weapons/Weapon")]
    public sealed class WeaponData : ScriptableObject, IData
    {
        public string Path { get; set; }
        
        #region Поля
        
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string m_prefabPath;

        [Header("Информация")]
        [SerializeField] private string _name = "Птичка";
        [SerializeField] [TextArea] private string _description = "Скорострельный пистолет-пулемёт";
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _price = 100;

        [Header("Характеристики")]
        [SerializeField] private int _damage = 10;
        
        [SerializeField] [Tooltip("Максимальное количество патрон в обойме")]
        private int _maxAmmo = 32;
        
        [SerializeField] [Tooltip("Скорострельность оружия (Промежуток между выстрелами в секундах)")] [Range(0.1f, 20f)]
        private float _fireRate = 0.1f;
        
        [SerializeField] private float _maxDistance = 100;
        
        [Header("Аудио")]
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _reloadClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _fireClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _noAmmoClipPath;
        
        [Header("Визуал")]
        [SerializeField] private Vector3 _reloadMove;

        #endregion
        
        #region Объекты
        
        private GameObject _prefab;
        
        private AudioClip _reloadClip;
        private AudioClip _fireClip;
        private AudioClip _noAmmoClip;
        
        #endregion
        
        #region Свойства

        // TODO: Добавить инвентарь для информации о оружии
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public int Price => _price;
        
        public int Damage => _damage;
        public int MaxAmmo => _maxAmmo;
        public float MaxDistance => _maxDistance;
        public float FireRate => _fireRate;
        
        public Vector3 ReloadMove => _reloadMove;

        public GameObject Prefab => GetData(m_prefabPath, ref _prefab);
        
        public AudioClip ReloadClip => GetData(_reloadClipPath, ref _reloadClip);
        public AudioClip FireClip => GetData(_fireClipPath, ref _fireClip);
        public AudioClip NoAmmoClip => GetData(_noAmmoClipPath, ref _noAmmoClip);

        #endregion
    }
}