using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using Code.Managers;
using UnityEngine;

namespace Code.Data.WeaponModifications
{
    [CreateAssetMenu(fileName = "Barrel Modificator", menuName = "Data/Weapons Modifications/Barrel")]
    internal sealed class BarrelModificatorData: ScriptableObject, IWeaponModificatorData
    {
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _modificatorPrefabPath;
        [SerializeField] [AssetPath.Attribute(typeof(AudioClip))] private string _fireClipPath;
        [SerializeField] [AssetPath.Attribute(typeof(Sprite))] private string _iconPath;
        
        [Header("Координаты")]
        [SerializeField] [Tooltip("Добавочные координаты к тем которые устанавливаются с помощью оружия")] 
        private Vector3 _additionalPosition;
        
        [Header("Прочее")]
        [SerializeField] private WeaponManager.WeaponType _weaponTypes;

        private AudioClip _audioClip;
        private GameObject _modificatorPrefab;
        private Sprite _icon;
        
        public GameObject ModificatorPrefab => GetData(_modificatorPrefabPath, _modificatorPrefab);
        public AudioClip FireClip => GetData(_fireClipPath, _audioClip);
        public Sprite Icon => GetData(_iconPath, _icon);

        public WeaponManager.WeaponType WeaponTypes => _weaponTypes;
        public Vector3 AdditionalPosition => _additionalPosition;
    }
}