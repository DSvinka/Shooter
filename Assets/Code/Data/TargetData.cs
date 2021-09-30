using Code.Interfaces.Data;
using Code.Managers;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Target", menuName = "Data/Units/Enemy/Target")]
    public sealed class TargetData : ScriptableObject, IData, IEnemyData
    {
        public string Path { get; set; }
        
        #region Поля
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _prefabPath;
        
        [Header("Характеристики")] 
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _maxArmor = 0f;

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
        public EnemyManager.EnemyType EnemyType => _enemyType;

        #endregion
    }
}