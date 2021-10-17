using System;
using System.Collections.Generic;
using Code.Bridges.Aims;
using Code.Bridges.Reloads;
using Code.Bridges.Weapon.Shoots;
using Code.Bridges.Weapon.Shoots.Cast;
using Code.Bridges.Weapon.Shoots.Damage;
using Code.Bridges.Weapon.Shoots.Shooting;
using Code.Controllers.Initialization;
using Code.Controllers.Player;
using Code.Data;
using Code.Data.DataStores;
using Code.Decorators;
using Code.Factory;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Bridges;
using Code.Interfaces.Bridges.Weapon.Shoots;
using Code.Interfaces.Input;
using Code.Managers;
using Code.Models;
using Code.Modifiers.WeaponAbility;
using Code.Services;
using Code.Views;
using RSG;
using UnityEngine;

using static Code.Utils.Extensions.Physic;

namespace Code.Controllers
{
    internal sealed class WeaponController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        private readonly WeaponFactory _weaponFactory;
        private readonly PoolService _poolService;
        private readonly IPromiseTimer _promiseTimer;

        private PlayerModel _player;

        private bool _reloadInput;
        private bool _aimInput;
        private bool _fireInput;

        private Vector3 _defaultAimPosition;
        
        private WeaponStore _weaponStore;

        public WeaponController(WeaponStore weaponStore, PlayerHudController playerHudController, PlayerInitialization playerInitialization, WeaponFactory weaponFactory, PoolService poolService, PromiseTimer promiseTimer)
        {
            _weaponStore = weaponStore;
            _weaponFactory = weaponFactory;
            _poolService = poolService;
            _promiseTimer = promiseTimer;

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
            if (_player.DefaultWeapon != null)
            {
                var gameObject = _player.DefaultWeapon.gameObject;
                gameObject.DisableAllPhysics(true);
                gameObject.DisableAllCollision();
                ChangeWeapon(_player.DefaultWeapon);
            }

            _reloadInputProxy.KeyOnDown += OnReloadInput;
            _fireInputProxy.KeyOnChange += OnFireInput;
            _aimInputProxy.KeyOnChange += OnAimInput;
        }
        
        public void Cleanup()
        {
            if (_player == null || _player.Weapon == null || _player.Weapon.View == null)
                return;

            _reloadInputProxy.KeyOnDown -= OnReloadInput;
            _fireInputProxy.KeyOnChange -= OnFireInput;
            _aimInputProxy.KeyOnChange -= OnAimInput;
        }
        
        public void ChangeWeapon(WeaponView view)
        {
            if (_player == null)
                return;

            var handPoint = _player.View.HandPoint;
            
            if (_defaultAimPosition != Vector3.zero)
                handPoint.localPosition = _defaultAimPosition;
            else
                _defaultAimPosition = handPoint.localPosition;

            var data = GetWeapon(view.WeaponType);

            var model = _weaponFactory.CreateWeapon(view, data);
            _player.Weapon = model;

            model.Transform.parent = handPoint;
            model.Transform.localPosition = Vector3.zero;
            model.Transform.localRotation = Quaternion.identity;

            var defaultProxy = GetProxy(model.Data.WeaponType);
            model.SetDefaultProxy(defaultProxy);
            model.ResetAllProxy();
            
            if (_player.Weapon.Data.DefaultBarrelModificator.ModificatorPrefab != null)
            {
                var modificationBarrel = new BarrelModification(_player.Weapon.Data.DefaultBarrelModificator, model.View.BarrelPosition);
                modificationBarrel.ApplyModification(model);
                model.SetShootProxy(modificationBarrel); 
            }

            if (_player.Weapon.Data.DefaultAimModificator.ModificatorPrefab != null)
            {
                var modificationAim = new AimModification(_player.Weapon.Data.DefaultAimModificator, model.View.AimPosition);
                modificationAim.ApplyModification(model);
                model.SetAimProxy(modificationAim);
            }

            _hudController.SetAmmo(model.BulletsLeft);
            _hudController.SetMaxAmmo(model.Data.MagazineSize);
        }

        private WeaponData GetWeapon(WeaponManager.WeaponType weaponType)
        {
            var weaponData = weaponType switch
            {
                WeaponManager.WeaponType.ShotGun => _weaponStore.ShotGunData,
                _ => throw new ArgumentOutOfRangeException(nameof(WeaponManager.WeaponType),
                    "Указанный тип оружия не предусмотрен в этом коде")
            };
            return weaponData;
        }
        
        private WeaponProxiesModel GetProxy(WeaponManager.WeaponType weaponType)
        {
            var weapon = _player.Weapon;
            IAim aimProxy;
            IReload reloadProxy;
            IShoot shootProxy;
            
            IDamage shootDamageProxy;
            ICast shootCastProxy;

            switch (weaponType)
            {
                case WeaponManager.WeaponType.ShotGun:
                    reloadProxy = new ShotGunReload(weapon, _hudController, _promiseTimer);
                    aimProxy = new ShotGunAim(_player, weapon);

                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), "Указанный тип оружия не привязан не к одному ShootProxy!");
            }
            
            switch (weapon.Data.ShootingType)
            {
                case ShootingType.Pistol:
                    shootProxy = new PistolShoot(_player, weapon, _hudController, _poolService, _promiseTimer);
                    break;
                case ShootingType.ShotGun:
                    shootProxy = new ShotGunShoot(_player, weapon, _hudController, _poolService, _promiseTimer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon.Data.ShootingType), "Указаный тип стрельбы не предусмотрен!");
            }

            switch (weapon.Data.DamageType)
            {
                case DamageType.Normal:
                    shootDamageProxy = new NormalDamage(_player, weapon);
                    break;
                case DamageType.Explosion:
                    shootDamageProxy = new ExplosionDamage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon.Data.DamageType), "Указаный тип урона не предусмотрен!");
            }

            switch (weapon.Data.CastType)
            {
                case CastType.Raycast:
                    shootCastProxy = new Raycast(_player, weapon, _poolService, _promiseTimer);
                    break;
                case CastType.Projectile:
                    shootCastProxy = new Projectile();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weapon.Data.DamageType), "Указаный тип проверки попадания не предусмотрен!");
            }

            var weaponProxiesModel = new WeaponProxiesModel();
            weaponProxiesModel.SetProxies(aimProxy, reloadProxy, shootProxy, shootCastProxy, shootDamageProxy);
            return weaponProxiesModel;
        }

        public void Execute(float deltaTime)
        {
            if (_player == null || _player.Weapon == null || _player.Weapon.View == null)
                return;

            var model = _player.Weapon;
            var proxies = model.Proxies;
            
            proxies.ShootProxy.MoveBullets(deltaTime);

            if (!model.Blocking)
            {
                if (_aimInput)
                    proxies.AimProxy.OpenAim();
            
                if (!_aimInput && model.IsAiming)
                    proxies.AimProxy.CloseAim();

                if (_fireInput)
                    proxies.ShootProxy.Shoot(deltaTime);
                
                if (_reloadInput)
                {
                    proxies.AimProxy.CloseAim();
                    proxies.ReloadProxy.Reload();
                }
            }

            if (model.FireCooldown >= 0f)
                model.FireCooldown -= deltaTime;
        }
    }
}