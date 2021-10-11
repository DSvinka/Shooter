using Code.Interfaces.Bridges;
using Code.Models;
using UnityEngine;

namespace Code.Bridges.Aims
{
    internal sealed class ShotGunAim: IAim
    {
        private PlayerModel _player;
        private WeaponModel _weapon;

        public ShotGunAim(PlayerModel playerModel, WeaponModel weaponModel)
        {
            _player = playerModel;
            _weapon = weaponModel;
        }
        
        public void OpenAim()
        {
            if (!_weapon.IsReloading)
            {
                _weapon.Transform.position = _player.View.AimPoint.position;
                _weapon.IsAiming = true;
            }
        }
        
        public void CloseAim()
        {
            if (_weapon.IsAiming || _weapon.IsReloading)
            {
                _weapon.Transform.localPosition = Vector3.zero;
                _weapon.IsAiming = false;
            }
        }
    }
}