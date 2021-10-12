using System.Collections;
using Code.Controllers;
using Code.Interfaces.Bridges;
using Code.Models;
using DG.Tweening;
using RSG;
using UnityEngine;

namespace Code.Bridges.Reloads
{
    internal sealed class ShotGunReload: IReload
    {
        private WeaponModel _weapon;
        private PlayerHudController _playerHudController;

        private IPromiseTimer _promiseTimer;
        
        public ShotGunReload(WeaponModel weaponModel, PlayerHudController playerHudController, IPromiseTimer promiseTimer)
        {
            _weapon = weaponModel;
            _playerHudController = playerHudController;
            _promiseTimer = promiseTimer;
        }
        
        public void Reload()
        {
            if (!_weapon.IsReloading && (_weapon.BulletsLeft != _weapon.Data.MagazineSize))
            {
                var startRotation = _weapon.Transform.localRotation.eulerAngles;

                ReloadStart(startRotation);
                _promiseTimer.WaitFor(_weapon.Data.ReloadClip.length)
                    .Then(() => ReloadEnd(startRotation));
            }
        }

        private void ReloadStart(Vector3 startRotation)
        {
            _weapon.IsReloading = true;
            _weapon.AudioSource.PlayOneShot(_weapon.Data.ReloadClip);
            
            _weapon.Transform.DOLocalRotate(startRotation + _weapon.Data.ReloadMove, 0.5f);
        }
        
        private void ReloadEnd(Vector3 startRotation)
        {
            _weapon.Transform.DOLocalRotate(startRotation, 0.5f);
            
            _weapon.BulletsLeft = _weapon.Data.MagazineSize;
            _weapon.IsReloading = false;
            
            _playerHudController.SetAmmo(_weapon.BulletsLeft);
        }
    }
}