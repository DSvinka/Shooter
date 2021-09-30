using System;
using System.Collections;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Controllers.ObjectPool;
using Code.Data;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Units;
using Code.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Controllers
{
    internal sealed class WeaponController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        private readonly PoolServices _poolServices;
        private PlayerView _player;
        private WeaponView _weapon;
        
        private Camera _camera;
        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;
        private Transform _transform;
        private WeaponData _data;

        private int _bulletsLeft;
        private float _fireCooldown;
        private bool _isReloading;
        private bool _isAiming;

        private bool _reloadInput;
        private bool _aimInput;
        private bool _fireInput;

        private Vector3 _screenCenter;
        private Vector3 _gunInitialPosition;
        private List<GameObject> _bullets;

        public WeaponController(PlayerHudController playerHudController, PlayerInitialization playerInitialization, PoolServices poolServices)
        {
            _hudController = playerHudController;
            _initialization = playerInitialization;
            _poolServices = poolServices;

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
            _camera = _player.GetComponentInChildren<Camera>();
            _screenCenter = new Vector3(0.5f, 0.5f, 0);
            _bullets = new List<GameObject>(32);

            _reloadInputProxy.KeyOnDown += OnReloadInput;
            _fireInputProxy.KeyOnChange += OnFireInput;
            _aimInputProxy.KeyOnChange += OnAimInput;
        }
        
        public void Cleanup()
        {
            if (_weapon != null)
                _weapon.StopAllCoroutines();
            
            _reloadInputProxy.KeyOnDown -= OnReloadInput;
            _fireInputProxy.KeyOnChange -= OnFireInput;
            _aimInputProxy.KeyOnChange -= OnAimInput;
        }
        
        public void ChangeWeapon(WeaponView weapon)
        {
            if (_weapon != null)
                _weapon.StopAllCoroutines();

            _weapon = weapon;
            _data = weapon.WeaponData;
            _particleSystem = weapon.GetComponentInChildren<ParticleSystem>();
            _audioSource = weapon.GetComponent<AudioSource>();
            _transform = weapon.transform;

            _bulletsLeft = _data.MagazineSize;
            _gunInitialPosition = _transform.localPosition;
            
            _hudController.SetAmmo(_bulletsLeft);
            _hudController.SetMaxAmmo(_data.MagazineSize);
        }

        public void Execute(float deltaTime)
        {
            if (_weapon == null)
                return;

            MoveBullets(deltaTime);
            Aim();
            Reload();
            Shoot();

            if (_fireCooldown >= 0f)
                _fireCooldown -= deltaTime;
        }
        
        private void MoveBullets(float deltatime)
        {
            for (var index = 0; index < _bullets.Count; index++)
            {
                var bullet = _bullets[index];
                bullet.transform.position += bullet.transform.forward * _data.BulletForce * deltatime;
            }
        }

        private void Aim()
        {
            if (_aimInput && !_isReloading)
            {
                _transform.localPosition = _player.AimPoint.localPosition;
                _isAiming = true;
            }
            else if (_isAiming && (!_aimInput || _isReloading))
            {
                _transform.localPosition = _gunInitialPosition;
                _isAiming = false;
            }
                
        }
        
        private void Reload()
        {
            if (_reloadInput && _bulletsLeft != _data.MagazineSize)
            {
                _weapon.StartCoroutine(ReloadTimer());
            }
        }

        private void Shoot()
        {
            if (_fireInput && _fireCooldown <= 0 && !_isReloading)
            {
                if (_bulletsLeft == 0)
                {
                    _audioSource.PlayOneShot(_data.NoAmmoClip);
                    _fireCooldown = _data.FireRate;
                    return;
                }

                _particleSystem.Play();
                _audioSource.PlayOneShot(_data.FireClip);

                var (origin, direction) = CalcDirection();
                TryDamage(origin, direction);
                CreateBullet(direction);

                _bulletsLeft -= 1;
                _hudController.SetAmmo(_bulletsLeft);
                _fireCooldown = _data.FireRate;
            }
        }

        private (Vector3 origin, Vector3 direction) CalcDirection()
        {
            var ray = _camera.ViewportPointToRay(_screenCenter);
            var targetPoint = ray.GetPoint(_data.MaxDistance);

            var spread = _data.Spread;
            if (_isAiming)
                spread = _data.SpreadAim;
                    
            var x = Random.Range(-spread, spread);
            var y = Random.Range(-spread, spread);

            var directionWithoutSpread = targetPoint - _weapon.ShotPoint.position;
            var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

            return (ray.origin, directionWithSpread);
        }

        private void TryDamage(Vector3 origin, Vector3 direction)
        {
            if (Physics.Raycast(origin, direction, out var hit, _data.MaxDistance, _data.LayerMask))
            {
                if (_player.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                {
                    var unit = hit.collider.gameObject.GetComponent<IUnit>();
                    unit?.AddDamage(_player.gameObject, _data.Damage);
                }
            }
        }

        private void CreateBullet(Vector3 direction)
        {
            var bullet = _poolServices.Instantiate(_weapon.WeaponData.BulletPrefab);
            var bulletTransform = bullet.transform;
            bulletTransform.position = _weapon.ShotPoint.position;
            bulletTransform.forward = direction.normalized;
            bullet.SetActive(true);

            var bulletView = bullet.AddComponent<BulletView>();
            if (bulletView == null)
                throw new Exception("Пуля не имеет BulletView");
            bulletView.OnCollision += OnBulletHit;
                
            _bullets.Add(bullet);
            _weapon.StartCoroutine(BulletLifetime(bullet));
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
            _poolServices.Destroy(bullet);
            _bullets.Remove(bullet);
        }
        
        private IEnumerator BulletLifetime(GameObject bullet)
        {
            yield return new WaitForSeconds(_weapon.WeaponData.BulletLifetime);
            if (bullet != null)
                DestroyBullet(bullet);
        }
        
        private IEnumerator ReloadTimer()
        {

            _isReloading = true;
            _audioSource.PlayOneShot(_data.ReloadClip);
            
            _transform.Rotate(_data.ReloadMove);

            yield return new WaitForSeconds(_data.ReloadClip.length);
            
            _transform.Rotate(-_data.ReloadMove);
            
            _bulletsLeft = _data.MagazineSize;
            _isReloading = false;
            _hudController.SetAmmo(_bulletsLeft);
        }
    }
}