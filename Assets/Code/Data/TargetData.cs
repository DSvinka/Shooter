using Code.Interfaces.Data;
using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data
{
    [CreateAssetMenu(fileName = "Target", menuName = "Data/Units/Target")]
    public sealed class TargetData : ScriptableObject, IData, IUnitData
    {
        public string Path { get; set; }
        
        #region Поля
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _targetPrefabPath;
        
        [Header("Характеристики")] 
        [SerializeField] private float _maxHealth = 50f;
        [SerializeField] private float _maxArmor = 0f;

        #endregion
        
        #region Объекты
        
        private GameObject _targetPrefab;

        #endregion
        
        #region Свойства
        
        public GameObject TargetPrefab => GetData(_targetPrefabPath, ref _targetPrefab);

        public float MaxHealth => _maxHealth;
        public float MaxArmor => _maxArmor;

        #endregion
    }
}