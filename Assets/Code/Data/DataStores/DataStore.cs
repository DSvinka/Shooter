using System.Collections.Generic;
using Code.Controllers.Starter;
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

        #endregion

        #region Объекты

        private PlayerData _player;
        private TargetData _target;
        
        private GameObject _playerHudPrefab;

        #endregion

        #region Свойства

        public PlayerData PlayerData => GetData(_playerDataPath, ref _player);
        public TargetData TargetData => GetData(_targetDataPath, ref _target);
        public GameObject PlayerHudPrefab => GetData(_playerHudPrefabPath, ref _playerHudPrefab);

        #endregion
    }
}