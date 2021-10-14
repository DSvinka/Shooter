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
        
        public IWeaponModification WeaponModification { get; }

        public BarrelModification(BarrelModificatorData data, Transform mufflerPosition)
        {
            WeaponModification = this;
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

        public void RemoveModification()
        {
            Object.Destroy(_barrel);
            _weapon.ResetAudioClip();
            _weapon.ResetBarrelPosition();
            _barrel = null;
        }

        public void ApplyModification(WeaponModel weapon)
        {
            _weapon = AddModification(weapon);
        }

        public void MoveBullets(float deltaTime)
        {
            _weapon.DefaultProxies.ShootProxy.MoveBullets(deltaTime);
        }

        public void Shoot(float deltaTime)
        {
            _weapon.DefaultProxies.ShootProxy.Shoot(deltaTime);
        }
    }
}