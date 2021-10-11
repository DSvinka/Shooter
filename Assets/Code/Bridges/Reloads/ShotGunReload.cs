using System.Collections;
using Code.Controllers;
using Code.Interfaces.Bridges;
using Code.Models;
using UnityEngine;

namespace Code.Bridges.Reloads
{
    internal sealed class ShotGunReload: IReload
    {
        private WeaponModel _weapon;
        private PlayerHudController _playerHudController;
        
        public ShotGunReload(WeaponModel weaponModel, PlayerHudController playerHudController)
        {
            _weapon = weaponModel;
            _playerHudController = playerHudController;
        }
        
        public void Reload()
        {
            if (!_weapon.IsReloading && (_weapon.BulletsLeft != _weapon.Data.MagazineSize))
            {
                _weapon.View.StartCoroutine(ReloadTimer());
            }
        }

        private IEnumerator ReloadTimer()
        {
            _weapon.IsReloading = true;
            _weapon.AudioSource.PlayOneShot(_weapon.Data.ReloadClip);
            
            _weapon.Transform.Rotate(_weapon.Data.ReloadMove);

            yield return new WaitForSeconds(_weapon.Data.ReloadClip.length);
            
            _weapon.Transform.Rotate(-_weapon.Data.ReloadMove);
            
            _weapon.BulletsLeft = _weapon.Data.MagazineSize;
            _weapon.IsReloading = false;
            
            _playerHudController.SetAmmo(_weapon.BulletsLeft);
        }
    }
}