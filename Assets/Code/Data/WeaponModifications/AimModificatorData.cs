using static Code.Data.DataUtils;
using Code.Interfaces.Data;
using Code.Managers;
using UnityEngine;

namespace Code.Data.WeaponModifications
{
    [CreateAssetMenu(fileName = "Aim Modificator", menuName = "Data/Weapons Modifications/Aim")]
    internal sealed class AimModificatorData: ScriptableObject, IWeaponModificatorData
    {
        [Header("Объекты")]
        [SerializeField] [AssetPath.Attribute(typeof(GameObject))] private string _modificatorPrefabPath;
        
        [Header("Координаты")]
        [SerializeField] [Tooltip("Добавочные координаты к тем которые устанавливаются оружием")] 
        private Vector3 _additionalPosition;
        [SerializeField] [Tooltip("Добавочные координаты прицеливания к тем которые устанавливаются оружием")] 
        private Vector3 _additionalAimPosition;
        
        [Header("Прочее")]
        [SerializeField] private WeaponManager.WeaponType _weaponTypes;
        
        private GameObject _modificatorPrefab;
        
        public GameObject ModificatorPrefab => GetData(_modificatorPrefabPath, _modificatorPrefab);
        
        public WeaponManager.WeaponType WeaponTypes => _weaponTypes;
        public Vector3 AdditionalAimPosition => _additionalAimPosition;
        public Vector3 AdditionalPosition => _additionalPosition;
    }
}