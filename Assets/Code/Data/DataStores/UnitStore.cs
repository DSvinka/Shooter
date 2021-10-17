using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "UnitStore", menuName = "Data/Data Stores/UnitStore")]
    internal sealed class UnitStore: ScriptableObject
    {
        #region Пути
        
        [Header("Союзники")]
        [SerializeField] [AssetPath.Attribute(typeof(PlayerData))] private string _playerDataPath;
        [Header("Противники")]
        [SerializeField] [AssetPath.Attribute(typeof(EnemyData))] private string _zombieDataPath;
        
        #endregion

        #region Объекты
        
        private PlayerData _playerData;
        private EnemyData _zombieData;
        
        #endregion
        
        #region Публичные Свойства
        
        public PlayerData PlayerData => GetData(_playerDataPath, _playerData);
        public EnemyData ZombieData => GetData(_zombieDataPath, _zombieData);
        
        #endregion
    }
}