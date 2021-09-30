using Code.Interfaces.Data;
using Code.Managers;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Melee", menuName = "Data/Units/Enemy/Melee")]
    public sealed class EnemyMeleeData : ScriptableObject, IData, IEnemyMeleeData
    {
        public string Path { get; set; }
        
        #region Поля
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _prefabPath;
        
        [Header("Характеристики")] 
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _maxArmor = 0f;
        
        [Header("Атака")] 
        [SerializeField] private float _attackDamage = 25f;
        [SerializeField] private float _attackDistance = 10f;
        [SerializeField] private float _attackRate = 2f;
        
        [Header("Метки")]
        [SerializeField] private EnemyManager.EnemyType _enemyType;

        #endregion
        
        #region Объекты
        
        private GameObject _prefab;

        #endregion
        
        #region Свойства
        
        public GameObject Prefab => GetData(_prefabPath, _prefab);

        public float MaxHealth => _maxHealth;
        public float MaxArmor => _maxArmor;

        public float AttackDamage => _attackDamage;
        public float AttackDistance => _attackDistance;
        public float AttackRate => _attackRate;
        public EnemyManager.EnemyType EnemyType => _enemyType;

        #endregion
    }
}