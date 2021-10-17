using Code.Data.WeaponModifications;
using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "DataStore", menuName = "Data/Data Stores/DataStore")]
    internal sealed class DataStore : ScriptableObject
    {
        #region Пути
        
        [SerializeField] [AssetPath.Attribute(typeof(UIStore))] private string _uiStorePath;
        [SerializeField] [AssetPath.Attribute(typeof(UnitStore))] private string _unitStorePath;
        [SerializeField] [AssetPath.Attribute(typeof(WeaponStore))] private string _weaponStorePath;
        [SerializeField] [AssetPath.Attribute(typeof(PhysicItemStore))] private string _physicItemStorePath;
        
        #endregion

        #region Объекты

        private UIStore _uiStore;
        private UnitStore _unitStore;
        private WeaponStore _weaponStore;
        private PhysicItemStore _physicItemStore;
        
        #endregion
        
        #region Публичные Свойства

        public UIStore UIStore => GetData(_uiStorePath, _uiStore);
        public UnitStore UnitStore => GetData(_unitStorePath, _unitStore);
        public WeaponStore WeaponStore => GetData(_weaponStorePath, _weaponStore);
        public PhysicItemStore PhysicItemStore => GetData(_physicItemStorePath, _physicItemStore);
        
        #endregion
    }
}