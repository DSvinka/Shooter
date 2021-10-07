using System;
using System.Collections;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Controllers.ObjectPool;
using Code.Data;
using Code.Factory;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Views;
using Code.Models;
using Code.Services;
using Code.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Controllers
{
    internal sealed class WeaponController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        private readonly WeaponFactory _weaponFactory;

        private PlayerModel _player;
        private WeaponModel _weapon;

        private bool _reloadInput;
        private bool _aimInput;
        private bool _fireInput;

        private Vector3 _screenCenter;
        private Vector3 _gunInitialPosition;
        private List<GameObject> _bullets;

        public WeaponController(PlayerHudController playerHudController, PlayerInitialization playerInitialization, WeaponFactory weaponFactory)
        {
            _weaponFactory = weaponFactory;
                
            _hudController = playerHudController;
            _initialization = playerInitialization;

            _reloadInputProxy = KeysInput.Reload;
            _fireInputProxy = MouseInput.Fire;
            _aimInputProxy = MouseInput.Aim;
        }

        #region Input Proxy
        
        private IUserKeyDownProxy _reloadInputProxy;
        private IUserKeyProxy _fireInputProxy;
        private IUserKeyProxy _aimInputProxy;
        
        private void OnReloadInput(bool value) => _reloadInput = value;
        private void OnFireInput(bool value) => _fireInput = value;
        private void OnAimInput(bool value) => _aimInput = value;
        
        #endregion

        public void Initialization()
        {
            _player = _initialization.GetPlayer();
            _screenCenter = new Vector3(0.5f, 0.5f, 0);
            _bullets = new List<GameObject>(32);

            _reloadInputProxy.KeyOnDown += OnReloadInput;
            _fireInputProxy.KeyOnChange += OnFireInput;
            _aimInputProxy.KeyOnChange += OnAimInput;
        }
        
        public void Cleanup()
        {
            var weaponView = _player.Weapon.View;
            if (weaponView != null)
                weaponView.StopAllCoroutines();
            
            _reloadInputProxy.KeyOnDown -= OnReloadInput;
            _fireInputProxy.KeyOnChange -= OnFireInput;
            _aimInputProxy.KeyOnChange -= OnAimInput;
        }
        
        public void ChangeWeapon(WeaponData weaponData)
        {
            if (_player.Weapon != null && _player.Weapon.View != null)
                _player.Weapon.View.StopAllCoroutines();

            var weaponModel = _weaponFactory.CreateWeapon(weaponData);
            
            _player.Weapon = weaponModel;
            _weapon = _player.Weapon;
            _weapon.Transform.parent = _player.CameraTransform;
            _weapon.Transform.position = _player.View.WeaponPoint.position;
            _gunInitialPosition = _weapon.Transform.localPosition;
            
            _hudController.SetAmmo(_weapon.BulletsLeft);
            _hudController.SetMaxAmmo(_weapon.Data.MagazineSize);
        }

        public void Execute(float deltaTime)
        {
            if (_weapon.View == null)
                return;

            MoveBullets(deltaTime);
            Aim();
            Reload();
            Shoot();

            if (_weapon.FireCooldown >= 0f)
                _weapon.FireCooldown -= deltaTime;
        }
        
        private void MoveBullets(float deltatime)
        {
            for (var index = 0; index < _bullets.Count; index++)
            {
                var bullet = _bullets[index];
                bullet.transform.position += bullet.transform.forward * _player.Weapon.Data.BulletForce * deltatime;
            }
        }

        private void Aim()
        {
            if (_aimInput && !_weapon.IsReloading)
            {
                _weapon.Transform.localPosition = _player.View.AimPoint.localPosition;
                _weapon.IsAiming = true;
            }
            else if (_weapon.IsAiming && (!_aimInput || _weapon.IsReloading))
            {
                _weapon.Transform.localPosition = _gunInitialPosition;
                _weapon.IsAiming = false;
            }
                
        }
        
        private void Reload()
        {
            if (_reloadInput && _weapon.BulletsLeft != _weapon.Data.MagazineSize)
            {
                _weapon.View.StartCoroutine(ReloadTimer());
            }
        }

        private void Shoot()
        {
            if (_fireInput && _weapon.FireCooldown <= 0 && !_weapon.IsReloading)
            {
                if (_weapon.BulletsLeft == 0)
                {
                    _weapon.AudioSource.PlayOneShot(_weapon.Data.NoAmmoClip);
                    _weapon.FireCooldown = _weapon.Data.FireRate;
                    return;
                }

                _weapon.ParticleSystem.Play();
                _weapon.AudioSource.PlayOneShot(_weapon.Data.FireClip);

                var (origin, direction) = CalcDirection();
                TryDamage(origin, direction);
                CreateBullet(direction);

                _weapon.BulletsLeft -= 1;
                _hudController.SetAmmo(_weapon.BulletsLeft);
                _weapon.FireCooldown = _weapon.Data.FireRate;
            }
        }

        private (Vector3 origin, Vector3 direction) CalcDirection()
        {
            var ray = _player.Camera.ViewportPointToRay(_screenCenter);
            var targetPoint = ray.GetPoint(_weapon.Data.MaxDistance);

            var spread = _weapon.Data.Spread;
            if (_weapon.IsAiming)
                spread = _weapon.Data.SpreadAim;
                    
            var x = Random.Range(-spread, spread);
            var y = Random.Range(-spread, spread);

            var directionWithoutSpread = targetPoint - _weapon.View.ShotPoint.position;
            var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            return (ray.origin, directionWithSpread);
        }

        private void TryDamage(Vector3 origin, Vector3 direction)
        {
            if (Physics.Raycast(origin, direction, out var hit, _weapon.Data.MaxDistance, _weapon.Data.LayerMask))
            {
                if (_player.View.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                {
                    var unit = hit.collider.gameObject.GetComponent<IUnitView>();
                    unit?.AddDamage(_player.View.gameObject, _weapon.Data.Damage);
                }
            }
        }

        private void CreateBullet(Vector3 direction)
        {
            var bullet = ServiceLocator.Resolve<PoolService>().Instantiate(_weapon.Data.BulletPrefab);
            var bulletTransform = bullet.transform;
            bulletTransform.position = _weapon.View.ShotPoint.position;
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
            bullet.OnCollision -= OnBulletHit;

            var bulletObject = bullet.gameObject;
            DestroyBullet(bulletObject);
        }
        
        private void DestroyBullet(GameObject bullet)
        {
            bullet.SetActive(false);
            ServiceLocator.Resolve<PoolService>().Destroy(bullet);
            _bullets.Remove(bullet);
        }
        
        private IEnumerator BulletLifetime(GameObject bullet)
        {
            yield return new WaitForSeconds(_weapon.Data.BulletLifetime);
            if (bullet != null)
                DestroyBullet(bullet);
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
            _hudController.SetAmmo(_weapon.BulletsLeft);
        }
    }
}