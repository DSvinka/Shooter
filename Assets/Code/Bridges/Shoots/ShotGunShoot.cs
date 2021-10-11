﻿using System;
using System.Collections;
using System.Collections.Generic;
using Code.Controllers;
using Code.Interfaces.Bridges;
using Code.Interfaces.Views;
using Code.Managers;
using Code.Models;
using Code.Services;
using Code.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Bridges.Shoots
{
    internal sealed class ShotGunShoot: IShoot
    {
        private PlayerModel _player;
        private WeaponModel _weapon;
        private PoolService _poolService;
        private PlayerHudController _hudController;
        private List<GameObject> _bullets;
        
        public ShotGunShoot(PlayerModel playerModel, WeaponModel weaponModel, PlayerHudController hudController, PoolService poolService)
        {
            _player = playerModel;
            _weapon = weaponModel;
            _poolService = poolService;
            _hudController = hudController;
            
            _bullets = new List<GameObject>(_weapon.Data.MagazineSize);
        }
        
        public void MoveBullets(float deltatime)
        {
            for (var index = 0; index < _bullets.Count; index++)
            {
                var bullet = _bullets[index];
                bullet.transform.position += bullet.transform.forward * _player.Weapon.Data.BulletForce * deltatime;
            }
        }
        
        // TODO: Сделать стрельбу как из дробовика.
        // TODO: Добавить звук перезарядки после выстрела.
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
            
            var (origin, direction) = CalcDirection(spread, _weapon.BarrelPosition.position);
            TryDamage(origin, direction, _weapon.Data.Damage);
            CreateBullet(direction);
            
            _weapon.BulletsLeft -= 1;
            _hudController.SetAmmo(_weapon.BulletsLeft);
            _weapon.FireCooldown = _weapon.Data.FireRate;
        }
        
        private (Vector3 origin, Vector3 direction) CalcDirection(float spread, Vector3 barrelPosition)
        {
            var ray = _player.Camera.ViewportPointToRay(VectorManager.ScreenCenter);
            var targetPoint = ray.GetPoint(_weapon.Data.MaxDistance);

            var x = Random.Range(-spread, spread);
            var y = Random.Range(-spread, spread);

            var directionWithoutSpread = targetPoint - barrelPosition;
            var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            return (ray.origin, directionWithSpread);
        }

        private void TryDamage(Vector3 origin, Vector3 direction, float damage)
        {
            if (Physics.Raycast(origin, direction, out var hit, _weapon.Data.MaxDistance, _weapon.Data.RayCastLayerMask))
            {
                if (_player.View.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                {
                    var unit = hit.collider.gameObject.GetComponent<IUnitView>();
                    unit?.AddDamage(_player.View.gameObject, damage);
                }
            }
        }

        private void CreateBullet(Vector3 direction)
        {
            var bullet = _poolService.Instantiate(_weapon.Data.BulletPrefab);
            var bulletTransform = bullet.transform;
            bulletTransform.position = _weapon.BarrelPosition.position;
            bulletTransform.forward = direction.normalized;
            bullet.SetActive(true);

            var bulletView = bullet.AddComponent<BulletView>();
            if (bulletView == null)
                throw new Exception("Пуля не имеет BulletView");
            bulletView.OnCollision += OnBulletHit;
                
            _bullets.Add(bullet);
            _weapon.View.StartCoroutine(BulletLifetime(bullet));
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
            bullet.SetActive(false);
            _poolService.Destroy(bullet);
            _bullets.Remove(bullet);
        }
        
        private IEnumerator BulletLifetime(GameObject bullet)
        {
            yield return new WaitForSeconds(_weapon.Data.BulletLifetime);
            if (bullet != null)
                DestroyBullet(bullet);
        }
        
        
    }
}