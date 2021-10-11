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
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class WeaponController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _initialization;
        private readonly PlayerHudController _hudController;
        private readonly WeaponFactory _weaponFactory;
        private readonly PoolService _poolService;

        private PlayerModel _player;

        private bool _reloadInput;
        private bool _aimInput;
        private bool _fireInput;

        private IReload _reloadProxy;
        private IShoot _shootProxy;
        private IAim _aimProxy;

        private Vector3 _defaultAimPosition;
        
        private DataStore _data;

        public WeaponController(DataStore data, PlayerHudController playerHudController, PlayerInitialization playerInitialization, PoolService poolService, WeaponFactory weaponFactory)
        {
            _data = data;
            _weaponFactory = weaponFactory;
            _poolService = poolService;

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
            
            var weaponView = _player.Weapon.View;
            if (weaponView != null)
                weaponView.StopAllCoroutines();
            
            _reloadInputProxy.KeyOnDown -= OnReloadInput;
            _fireInputProxy.KeyOnChange -= OnFireInput;
            _aimInputProxy.KeyOnChange -= OnAimInput;
        }
        
        public void ChangeWeapon(WeaponManager.WeaponType weaponType)
        {
            if (_player == null)
                return;
            
            if (_player.Weapon != null && _player.Weapon.View != null)
                _player.Weapon.View.StopAllCoroutines();

            var weaponPoint = _player.View.WeaponPoint;
            
            if (_defaultAimPosition != Vector3.zero)
                weaponPoint.localPosition = _defaultAimPosition;
            else
                _defaultAimPosition = weaponPoint.localPosition;

            var weaponData = GetWeapon(weaponType);

            var weaponModel = _weaponFactory.CreateWeapon(weaponData);
            _player.Weapon = weaponModel;

            weaponModel.Transform.parent = weaponPoint;
            weaponModel.Transform.localPosition = Vector3.zero;
            weaponModel.Transform.localRotation = Quaternion.identity;

            var (reloadProxy, shootProxy, aimProxy) = GetProxy(weaponModel.Data.WeaponType);
            weaponModel.SetShootProxy(shootProxy);
            weaponModel.SetAimProxy(aimProxy);
            weaponModel.SetReloadProxy(reloadProxy);
            _reloadProxy = weaponModel.ReloadProxy;
            _shootProxy = weaponModel.ShootProxy;
            _aimProxy = weaponModel.AimProxy;


            // TODO: Сделать снятие и надевание модулей. (можно как в батле 2042)
            var modificationBarrel = new BarrelModification(_data.MufflerModificator, weaponModel.View.BarrelPosition);
            modificationBarrel.ApplyModification(weaponModel);
            _shootProxy = modificationBarrel;
            
            // TODO: Сделать снятие и надевание модулей. (можно как в батле 2042)
            var modificationAim = new AimModification(_data.OpticalAimModificator, weaponModel.View.AimPosition);
            modificationAim.ApplyModification(weaponModel);
            _aimProxy = modificationAim;

            weaponPoint.localPosition += _data.OpticalAimModificator.AdditionalAimPosition;

            _hudController.SetAmmo(weaponModel.BulletsLeft);
            _hudController.SetMaxAmmo(weaponModel.Data.MagazineSize);
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
                    reloadProxy = new ShotGunReload(weapon, _hudController);
                    shootProxy = new ShotGunShoot(_player, weapon, _hudController, _poolService);
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

            _shootProxy.MoveBullets(deltaTime);
            
            _aimProxy.Aim(_aimInput);

            if (_fireInput)
                _shootProxy.Shoot(deltaTime);
            
            if (_reloadInput)
                _reloadProxy.Reload();

            if (_player.Weapon.FireCooldown >= 0f)
                _player.Weapon.FireCooldown -= deltaTime;
        }
    }
}