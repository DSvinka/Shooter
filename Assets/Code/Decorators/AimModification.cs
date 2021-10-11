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

        public AimModification(AimModificatorData data, Transform mufflerPosition)
        {
            _spawnPoint = mufflerPosition;
            _data = data;
        }
        
        public WeaponModel AddModification(WeaponModel weapon)
        {
            _aim = Object.Instantiate(_data.ModificatorPrefab, _spawnPoint);
            _aim.transform.localPosition += _data.AdditionalPosition;
            return weapon;
        }
        public WeaponModel RemoveModification(WeaponModel weapon)
        {
            return weapon;
        }
        
        public void ApplyModification(WeaponModel weapon)
        {
            _weapon = AddModification(weapon);
        }

        public void Aim(bool input)
        {
            _weapon.AimProxy.Aim(input);
        }
    }
}