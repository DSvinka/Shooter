using static Code.Data.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "DataStore", menuName = "Data/Data Stores/DataStore")]
    internal sealed class DataStore : ScriptableObject
    {
        #region Поля
        
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _playerHudPrefabPath;
        
        [Header("Data")]
        [SerializeField] [AssetPath.Attribute(typeof(PlayerData))] private string _playerDataPath;
        [SerializeField] [AssetPath.Attribute(typeof(TargetData))] private string _targetDataPath;
        [SerializeField] [AssetPath.Attribute(typeof(EnemyMeleeData))] private string _zombieDataPath;
        
        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string _stickGunPath;

        #endregion

        #region Объекты

        private PlayerData _player;
        private TargetData _target;
        private EnemyMeleeData _zombie;
        
        private GameObject _playerHudPrefab;

        private WeaponData _stickGun;

        #endregion

        #region Свойства

        public PlayerData PlayerData => GetData(_playerDataPath, _player);
        public TargetData TargetData => GetData(_targetDataPath, _target);
        public EnemyMeleeData ZombieData => GetData(_zombieDataPath, _zombie);
        public GameObject PlayerHudPrefab => GetData(_playerHudPrefabPath, _playerHudPrefab);
        public WeaponData StickGunData => GetData(_stickGunPath, _stickGun);

        #endregion
    }
}