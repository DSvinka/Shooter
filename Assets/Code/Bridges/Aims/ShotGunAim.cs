using Code.Decorators;
using Code.Interfaces.Bridges;
using Code.Models;
using DG.Tweening;
using UnityEngine;

namespace Code.Bridges.Aims
{
    internal sealed class ShotGunAim: IAim
    {
        public IWeaponModification WeaponModification { get; }
        
        private PlayerModel _player;
        private WeaponModel _weapon;

        public ShotGunAim(PlayerModel playerModel, WeaponModel weaponModel)
        {
            _player = playerModel;
            _weapon = weaponModel;
            WeaponModification = null;
        }

        public void OpenAim()
        {
            if (!_weapon.IsAiming && !_weapon.IsReloading)
            {
                _weapon.Transform.DOLocalMove(_player.View.AimPoint.localPosition, 0.3f);
                _weapon.IsAiming = true;
            }
        }
        
        public void CloseAim()
        {
            if (_weapon.IsAiming || _weapon.IsReloading)
            {
                _weapon.Transform.DOLocalMove(Vector3.zero, 0.3f);
                _weapon.IsAiming = false;
            }
        }
    }
}