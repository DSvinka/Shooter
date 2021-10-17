using static Code.Utils.Extensions.DataUtils;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data.WeaponModifications
{
    [CreateAssetMenu(fileName = "Aim Modificator", menuName = "Data/Weapons Modifications/Aim")]
    internal sealed class AimModificatorData: ScriptableObject, IWeaponModificatorData
    {
        public string Path { get; set; }
        
        #region Пути
        
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _modificatorPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(Sprite))] private string _iconPath;
        
        #endregion
        
        #region Поля
        
        [Header("Координаты")]
        [SerializeField] [Tooltip("Добавочные координаты к тем которые устанавливаются оружием")] 
        private Vector3 _additionalPosition;
        [SerializeField] [Tooltip("Добавочные координаты прицеливания к тем которые устанавливаются оружием")] 
        private Vector3 _additionalAimPosition;
        
        #endregion
        
        #region Объекты

        private GameObject _modificatorPrefab;
        private Sprite _icon;
        
        #endregion
        
        #region Публичные Свойства
        
        public GameObject ModificatorPrefab => GetData(_modificatorPrefabPath, _modificatorPrefab);
        public Sprite Icon => GetData(_iconPath, _icon);
        
        public Vector3 AdditionalAimPosition => _additionalAimPosition;
        public Vector3 AdditionalPosition => _additionalPosition;
        
        #endregion
    }
}