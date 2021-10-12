using Code.Data.WeaponModifications;
using Code.Interfaces.Bridges;
using Code.Models;
using UnityEngine;

namespace Code.Decorators
{
    internal sealed class AimModification: IWeaponModification, IAim
    {
        private WeaponModel _weapon;
        private readonly AimModificatorData _data;
        private readonly Transform _spawnPoint;
        private GameObject _aim;
        
        public IWeaponModification WeaponModification { get; }

        public AimModification(AimModificatorData data, Transform mufflerPosition)
        {
            WeaponModification = this;
            _spawnPoint = mufflerPosition;
            _data = data;
        }
        
        public WeaponModel AddModification(WeaponModel weapon)
        {
            _aim = Object.Instantiate(_data.ModificatorPrefab, _spawnPoint);
            _aim.transform.localPosition += _data.AdditionalPosition;
            return weapon;
        }
        public void RemoveModification()
        {
            Object.Destroy(_aim);
            _aim = null;
        }
        
        public void ApplyModification(WeaponModel weapon)
        {
            _weapon = AddModification(weapon);
        }

        public void OpenAim()
        {
            _weapon.AimDefaultProxy.OpenAim();
        }
        
        public void CloseAim()
        {
            _weapon.AimDefaultProxy.CloseAim();
        }
    }
}