using static Code.Utils.Extensions.DataUtils;
using UnityEngine;

namespace Code.Data.DataStores
{
    [CreateAssetMenu(fileName = "UIStore", menuName = "Data/Data Stores/UIStore")]
    internal sealed class UIStore: ScriptableObject
    {
        #region Пути
        
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _hudPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _escapeMenuPrefabPath;

        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _weaponModificationIconPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _notifyMessagePrefabPath;
        
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _hitMessagePrefabPath;
        
        #endregion
 
        #region Объекты
        
        private GameObject _hudPrefab;
        private GameObject _escapeMenuPrefab;

        private GameObject _settingsWindowPrefab;
        
        private GameObject _weaponModificationIconPrefab;
        private GameObject _notifyMessagePrefab;

        private GameObject _hitMessagePrefab;

        #endregion
        
        #region Публичные Свойства
        
        public GameObject HudPrefab => GetData(_hudPrefabPath, _hudPrefab);
        public GameObject EscapeMenuPrefab => GetData(_escapeMenuPrefabPath, _escapeMenuPrefab);

        public GameObject WeaponModificationIconPrefab => GetData(_weaponModificationIconPrefabPath, _weaponModificationIconPrefab);
        public GameObject NotifyMessagePrefab => GetData(_notifyMessagePrefabPath, _notifyMessagePrefab);

        public GameObject HitMessagePrefab => GetData(_hitMessagePrefabPath, _hitMessagePrefab);

        #endregion
    }
}