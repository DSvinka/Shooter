using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "WeaponStore", menuName = "Data/Data Stores/WeaponStore")]
    internal sealed class WeaponStore: ScriptableObject
    {
        #region Пути
        
        [Header("Среднее Оружие")]
        [SerializeField] [AssetPath.Attribute(typeof(WeaponData))] private string _shotGunDataPath;
        
        #endregion

        #region Объекты

        private WeaponData _shotGunData;
        
        #endregion
        
        #region Публичные Свойства
        
        public WeaponData ShotGunData => GetData(_shotGunDataPath, _shotGunData);
        
        #endregion
    }
}