using System;
using System.Collections.Generic;
using Code.Interfaces.Bridges.Weapon.Shoots;
using Code.Models;
using Code.Services;
using Code.Utils.Extensions;
using Code.Views;
using RSG;
using UnityEngine;

namespace Code.Bridges.Weapon.Shoots.Cast
{
    internal sealed class Raycast: ICast
    {
        private PlayerModel _player;
        private WeaponModel _weapon;
        private PoolService _poolService;
        private IPromiseTimer _promiseTimer;
        
        public List<GameObject> Bullets { get; set; }

        public Raycast(PlayerModel player, WeaponModel weapon, PoolService poolService, IPromiseTimer promiseTimer)
        {
            _player = player;
            _weapon = weapon;
            _poolService = poolService;
            _promiseTimer = promiseTimer;

            Bullets = new List<GameObject>();
        }
        
        public void Cast(Vector3 origin, Vector3 direction)
        {
            if (Physics.Raycast(origin, direction, out var hit, _weapon.Data.MaxDistance, _weapon.Data.RayCastLayerMask))
            {
                if (_player.View.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                {
                    _weapon.Proxies.ShootDamageProxy.Damage(hit.collider.gameObject, hit.point);
                }
            }
            BulletShoot(direction);
        }

        private void BulletShoot(Vector3 direction)
        {
            var bullet = _poolService.Instantiate(_weapon.Data.BulletPrefab);
            var bulletTransform = bullet.transform;
            bulletTransform.position = _weapon.BarrelPosition.position;
            bulletTransform.forward = direction.normalized;
            bullet.SetActive(true);
            
            if (!bullet.TryGetComponent(out BulletView bulletView))
                throw new Exception("Пуля не имеет BulletView");
            bulletView.OnCollision += OnBulletHit;
                
            Bullets.Add(bullet);
            _promiseTimer.WaitFor(_weapon.Data.BulletLifetime).Then(() => DestroyBullet(bullet));
        }
        
        private void OnBulletHit(BulletView bullet, GameObject hit)
        {
            if (hit.gameObject.GetInstanceID() == _weapon.View.GetInstanceID())
                return;
            
            bullet.OnCollision -= OnBulletHit;

            var bulletObject = bullet.gameObject;
            DestroyBullet(bulletObject);
        }
        
        private void DestroyBullet(GameObject bullet)
        {
            if (bullet == null)
                return;
            
            bullet.SetActive(false);
            bullet.GetComponentInChildren<TrailRenderer>().Clear();
            _poolService.Destroy(bullet);
            Bullets.Remove(bullet);
        }
    }
}