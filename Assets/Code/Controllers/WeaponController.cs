using System;
using Code.Bridges.Aims;
using Code.Bridges.Reloads;
using Code.Bridges.Shoots;
using Code.Controllers.Initialization;
using Code.Data;
using Code.Data.DataStores;
using Code.Decorators;
using Code.Factory;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Bridges;
using Code.Interfaces.Input;
using Code.Managers;
using Code.Models;
using Code.Services;
using Code.Views;
using RSG;
using UnityEngine;

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
        
        private DataStore _data;

        public WeaponController(DataStore data, PlayerHudController playerHudController, PlayerInitialization playerInitialization, WeaponFactory weaponFactory, PoolService poolService, PromiseTimer promiseTimer)
        {
            _data = data;
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

            var (reloadProxy, shootProxy, aimProxy) = GetProxy(model.Data.WeaponType);
            model.SetDefaultProxy(reloadProxy, shootProxy, aimProxy);
            model.ResetAllProxy();

            // TODO: Сделать снятие и надевание модулей. (можно как в батле 2042)
            var modificationBarrel = new BarrelModification(_data.MufflerModificator, model.View.BarrelPosition);
            modificationBarrel.ApplyModification(model);
            model.SetShootProxy(modificationBarrel);
            
            // TODO: Сделать снятие и надевание модулей. (можно как в батле 2042)
            var modificationAim = new AimModification(_data.OpticalAimModificator, model.View.AimPosition);
            modificationAim.ApplyModification(model);
            model.SetAimProxy(modificationAim);

            handPoint.localPosition += _data.OpticalAimModificator.AdditionalAimPosition;

            _hudController.SetAmmo(model.BulletsLeft);
            _hudController.SetMaxAmmo(model.Data.MagazineSize);
        }

        private WeaponData GetWeapon(WeaponManager.WeaponType weaponType)
        {
            var weaponData = weaponType switch
            {
                WeaponManager.WeaponType.ShotGun => _data.ShotGunData,
                _ => throw new ArgumentOutOfRangeException(nameof(WeaponManager.WeaponType),
                    "Указанный тип оружия не предусмотрен в этом коде")
            };
            return weaponData;
        }
        
        private (IReload, IShoot, IAim) GetProxy(WeaponManager.WeaponType weaponType)
        {
            var weapon = _player.Weapon;
            IReload reloadProxy;
            IShoot shootProxy;
            IAim aimProxy;

            switch (weaponType)
            {
                case WeaponManager.WeaponType.ShotGun:
                    reloadProxy = new ShotGunReload(weapon, _hudController, _promiseTimer);
                    shootProxy = new ShotGunShoot(_player, weapon, _hudController, _poolService, _promiseTimer);
                    aimProxy = new ShotGunAim(_player, weapon);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), "Указанный тип оружия не привязан не к одному ShootProxy!");
            }

            return (reloadProxy, shootProxy, aimProxy);
        }

        public void Execute(float deltaTime)
        {
            if (_player == null || _player.Weapon == null || _player.Weapon.View == null)
                return;

            var model = _player.Weapon;
            
            model.ShootProxy.MoveBullets(deltaTime);
            
            if (_aimInput)
                model.AimProxy.OpenAim();
            
            if (!_aimInput && model.IsAiming)
                model.AimProxy.CloseAim();

            if (_fireInput)
                model.ShootProxy.Shoot(deltaTime);

            if (_reloadInput)
            {
                model.AimProxy.CloseAim();
                model.ReloadProxy.Reload();
            }

            if (model.FireCooldown >= 0f)
                model.FireCooldown -= deltaTime;
        }
    }
}