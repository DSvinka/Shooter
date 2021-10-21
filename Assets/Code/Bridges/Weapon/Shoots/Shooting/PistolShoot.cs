using System.Collections.Generic;
using Code.Controllers.Player;
using Code.Decorators;
using Code.Interfaces.Bridges.Weapon.Shoots;
using Code.Managers;
using Code.Models;
using Code.Services;
using RSG;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Bridges.Weapon.Shoots.Shooting
{
    internal sealed class PistolShoot: IShoot
    {
        private PlayerModel _player;
        private WeaponModel _weapon;

        public IWeaponModification WeaponModification { get; }
        
        private PlayerHudController _hudController;

        public PistolShoot(PlayerModel playerModel, WeaponModel weaponModel, PlayerHudController hudController, PoolService poolService, IPromiseTimer promiseTimer)
        {
            _player = playerModel;
            _weapon = weaponModel;
            _hudController = hudController;

            WeaponModification = null;
        }

        public void MoveBullets(float deltatime)
        {
            var bullets = _weapon.Proxies.ShootCastProxy.Bullets;
            
            for (var index = 0; index < bullets.Count; index++)
            {
                var bullet = bullets[index];
                bullet.transform.position += bullet.transform.forward * _player.Weapon.Data.BulletForce * deltatime;
            }
        }
        
        public void Shoot(float deltaTime)
        {
            if (_weapon.FireCooldown >= 0 || _weapon.IsReloading)
                return;
            
            if (_weapon.BulletsLeft == 0)
            {
                _weapon.AudioSource.PlayOneShot(_weapon.Data.NoAmmoClip);
                _weapon.FireCooldown = _weapon.Data.FireRate;
                return;
            }
            
            _weapon.ParticleSystem.Play();
            _weapon.AudioSource.PlayOneShot(_weapon.FireClip);
            
            var spread = _weapon.Data.Spread;
            if (_weapon.IsAiming)
                spread = _weapon.Data.SpreadAim;
            
            var (origin, direction) = CalcDirection(spread, _weapon.BarrelPosition);
            _weapon.Proxies.ShootCastProxy.Cast(origin, direction);

            _weapon.BulletsLeft -= 1;
            _hudController.SetAmmo(_weapon.BulletsLeft);
            _weapon.FireCooldown = _weapon.Data.FireRate;
        }
        
        private (Vector3 origin, Vector3 direction) CalcDirection(float spread, Transform barrel)
        {
            var ray = _player.Camera.ViewportPointToRay(VectorManager.ScreenCenter);
            var targetPoint = ray.GetPoint(_weapon.Data.MaxDistance);

            var x = Random.Range(-spread, spread);
            var y = Random.Range(-spread, spread);

            var directionWithoutSpread = targetPoint - barrel.position;
            var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            return (ray.origin, directionWithSpread);
        }
    }
}