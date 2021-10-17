using static Code.Utils.Extensions.DataUtils;
using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Data.WeaponModifications
{
    [CreateAssetMenu(fileName = "Barrel Modificator", menuName = "Data/Weapons Modifications/Barrel")]
    internal sealed class BarrelModificatorData: ScriptableObject, IWeaponModificatorData
    {
        public string Path { get; set; }
        
        #region Пути
        
        [Header("Префабы")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _modificatorPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _fireClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(Sprite))] private string _iconPath;
        
        #endregion
        
        #region Поля
        
        [Header("Координаты")]
        [SerializeField] [Tooltip("Добавочные координаты к тем которые устанавливаются с помощью оружия")] 
        private Vector3 _additionalPosition;
        
        #endregion

        #region Объекты
        
        private AudioClip _audioClip;
        private GameObject _modificatorPrefab;
        private Sprite _icon;
        
        #endregion
        
        #region Публичные Свойства
        
        public GameObject ModificatorPrefab => GetData(_modificatorPrefabPath, _modificatorPrefab);
        public AudioClip FireClip => GetData(_fireClipPath, _audioClip);
        public Sprite Icon => GetData(_iconPath, _icon);
        
        public Vector3 AdditionalPosition => _additionalPosition;
        
        #endregion
    }
}