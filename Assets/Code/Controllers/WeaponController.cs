using System.Collections;
using Code.Controllers.Initialization;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Units;
using Code.Utils.Extensions;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class WeaponController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        private PlayerView _player;
        private WeaponView _weapon;
        
        private int _ammo;
        private float _cooldown;
        private bool _isReloading;

        private ParticleSystem _particleSystem;
        private AudioSource _audioSource;

        private bool _reloadInput;
        private bool _fireInput;

        public WeaponController(PlayerHudController playerHudController, PlayerInitialization playerInitialization)
        {
            _hudController = playerHudController;
            _initialization = playerInitialization;

            _reloadInputProxy = KeysInput.Reload;
            _fireInputProxy = MouseInput.Fire;
        }

        #region Input Proxy
        
        private IUserKeyDownProxy _reloadInputProxy;
        private IUserKeyProxy _fireInputProxy;
        
        private void OnReloadInput(bool value) => _reloadInput = value;
        private void OnFireInput(bool value) => _fireInput = value;

        #endregion

        public void Initialization()
        {
            _player = _initialization.GetPlayer();

            _reloadInputProxy.KeyOnDown += OnReloadInput;
            _fireInputProxy.KeyOnChange += OnFireInput;

            if (_weapon != null)
                _hudController.SetMaxAmmo(_weapon.WeaponData.MaxAmmo);
        }
        
        public void Cleanup()
        {
            if (_weapon != null)
                _weapon.StopAllCoroutines();
            
            _reloadInputProxy.KeyOnDown -= OnReloadInput;
            _fireInputProxy.KeyOnChange -= OnFireInput;
        }

        public void Execute(float deltaTime)
        {
            if (_weapon == null)
                return;
            
            Reload();
            Shoot();

            if (_cooldown >= 0f)
                _cooldown -= deltaTime;
        }

        public void ChangeWeapon(WeaponView weapon)
        {
            _weapon = weapon;
            _particleSystem = _weapon.GetComponentInChildren<ParticleSystem>();
            _audioSource = _weapon.GetComponent<AudioSource>();
            
            _hudController.SetMaxAmmo(_weapon.WeaponData.MaxAmmo);
        }

        private void Reload()
        {
            if (_reloadInput && _ammo != _weapon.WeaponData.MaxAmmo)
            {
                _weapon.StartCoroutine(ReloadTimer());
            }
        }

        private IEnumerator ReloadTimer()
        {
            var transform = _weapon.transform;
            var data = _weapon.WeaponData;
            
            _isReloading = true;
            _audioSource.PlayOneShot(data.ReloadClip);
            
            transform.Rotate(data.ReloadMove);

            yield return new WaitForSeconds(data.ReloadClip.length);
            
            transform.Rotate(-data.ReloadMove);
            
            _ammo = data.MaxAmmo;
            _isReloading = false;
            _hudController.SetAmmo(_ammo);
        }

        private void Shoot()
        {
            if (_fireInput && _cooldown <= 0 && !_isReloading)
            {
                var data = _weapon.WeaponData;
                
                if (_ammo == 0)
                {
                    _audioSource.PlayOneShot(data.NoAmmoClip);
                    _cooldown = data.FireRate;
                    return;
                }

                var barrelPosition = _weapon.ShotPoint.position;
                var barrelForward = _weapon.ShotPoint.forward;
                
                _particleSystem.Play();
                _audioSource.PlayOneShot(data.FireClip);

                if (Physics.Raycast(barrelPosition, barrelForward, out var raycastHit, data.MaxDistance))
                {
                    if (_player.gameObject.GetInstanceID() != raycastHit.collider.gameObject.GetInstanceID())
                    {
                        var unit = raycastHit.collider.gameObject.GetComponent<IUnit>();
                        unit?.AddDamage(_player.gameObject, data.Damage);
                    }
                }
                
                _ammo -= 1;
                _hudController.SetAmmo(_ammo);
                _cooldown = data.FireRate;
            }
        }
    }
}