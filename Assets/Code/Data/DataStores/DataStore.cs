using System;
using System.Collections.Generic;
using Code.Data.WeaponModifications;
using Code.Utils.Extensions;
using UnityEditor;
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
        
        [Header("Units")]
        [SerializeField] [AssetPath.Attribute(typeof(PlayerData))] private string _playerDataPath;
        [SerializeField] [AssetPath.Attribute(typeof(EnemyData))] private string _zombieDataPath;
        
        [Header("Weapons")]
        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string _shotGunPath;

        [Header("Weapons Modificators")]
        [SerializeField] [AssetPath.Attribute(typeof(BarrelModificatorData))] private string _mufflerPath;
        [SerializeField] [AssetPath.Attribute(typeof(AimModificatorData))] private string _opticalAimPath;

        #endregion

        #region Объекты

        private PlayerData _player;
        private EnemyData _zombie;
        
        private GameObject _playerHudPrefab;
        
        private WeaponData _shotGun;

        private BarrelModificatorData _muffler;
        private AimModificatorData _opticalAim;

        #endregion

        #region Свойства

        public PlayerData PlayerData => GetData(_playerDataPath, _player);
        public EnemyData ZombieData => GetData(_zombieDataPath, _zombie);
        public GameObject PlayerHudPrefab => GetData(_playerHudPrefabPath, _playerHudPrefab);
        
        public WeaponData ShotGunData => GetData(_shotGunPath, _shotGun);

        public BarrelModificatorData MufflerModificator => GetData(_mufflerPath, _muffler);
        public AimModificatorData OpticalAimModificator => GetData(_opticalAimPath, _opticalAim);

        #endregion
    }
}