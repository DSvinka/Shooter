using Code.Data.WeaponModifications;
using Code.Interfaces.Bridges;
using Code.Models;
using UnityEngine;

namespace Code.Decorators
{
    internal sealed class BarrelModification : IWeaponModification, IShoot
    {
        private WeaponModel _weapon;
        private readonly BarrelModificatorData _data;
        private readonly Transform _spawnPoint;
        private GameObject _barrel;

        public BarrelModification(BarrelModificatorData data, Transform mufflerPosition)
        {
            _spawnPoint = mufflerPosition;
            _data = data;
        }
        
        public WeaponModel AddModification(WeaponModel weapon)
        {
            _barrel = Object.Instantiate(_data.ModificatorPrefab, _spawnPoint);
            _barrel.transform.localPosition += _data.AdditionalPosition;
            weapon.SetAudioClip(_data.FireClip);
            weapon.SetBarrelPosition(_barrel.transform);
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
        
        public void MoveBullets(float deltaTime)
        {
            _weapon.ShootDefaultProxy.MoveBullets(deltaTime);
        }

        public void Shoot(float deltaTime)
        {
            _weapon.ShootDefaultProxy.Shoot(deltaTime);
        }
    }
}