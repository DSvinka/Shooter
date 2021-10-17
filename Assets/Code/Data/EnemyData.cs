using Code.Interfaces.Data;
using Code.Managers;
using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Data/Units/Enemy/Melee")]
    public sealed class EnemyData : ScriptableObject, IData, IEnemyData
    {
        public string Path { get; set; }
        
        #region Пути
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _prefabPath;
        
        [Header("Аудио Клипы")]
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _attackClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _getDamageClipPath;
        
        #endregion
        
        #region Поля
        
        [Header("Информация")] 
        [SerializeField] private string _name = "Противник";
        
        [Header("Характеристики")] 
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _maxArmor = 0f;
        
        [Header("Атака")] 
        [SerializeField] private float _attackDamage = 25f;
        [SerializeField] private float _attackDistance = 10f;
        [SerializeField] private float _attackRate = 2f;

        [Header("Аудио")]
        [SerializeField] private float _maxRandomSoundPitch = 2f;
        [SerializeField] private float _minRandomSoundPitch = 1f;
        
        [Header("Прочее")]
        [SerializeField] private int _scoreOnDeath;
        [SerializeField] private EnemyManager.EnemyType _enemyType;

        #endregion
        
        #region Объекты
        
        private GameObject _prefab;
        
        private AudioClip _attackClip;
        private AudioClip _getDamageClip;

        #endregion
        
        #region Публичные Свойства
        
        public GameObject Prefab => GetData(_prefabPath, _prefab);

        public string Name => _name;

        public float MaxHealth => _maxHealth;
        public float MaxArmor => _maxArmor;

        public float AttackDamage => _attackDamage;
        public float AttackDistance => _attackDistance;
        public float AttackRate => _attackRate;
        
        public AudioClip AttackClip => GetData(_attackClipPath, _attackClip);
        public AudioClip GetDamageClip => GetData(_getDamageClipPath, _getDamageClip);

        public float MaxRandomSoundPitch => _maxRandomSoundPitch;
        public float MinRandomSoundPitch => _minRandomSoundPitch;
        public int ScoreOnDeath => _scoreOnDeath;

        public EnemyManager.EnemyType EnemyType => _enemyType;

        #endregion
    }
}