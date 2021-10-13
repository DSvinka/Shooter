using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Data.DataStores;
using Code.Data.WeaponModifications;
using Code.Decorators;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Input;
using Code.Models;
using Code.Utils.Extensions;
using Code.Views;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    internal sealed class WeaponHudController: IController, IExecute, IInitialization, ICleanup
    {
        private BarrelModificatorData[] _barrelModificators;
        private AimModificatorData[] _aimModificators;
        
        // TODO: Может сделать модельку данных?
        private Dictionary<int, IWeaponModificatorData> _modificators;
        private Dictionary<int, Button> _buttons;

        private readonly PlayerInitialization _playerInitialization;
        private PlayerHudView _playerHudView;
        private GameObject _iconPrefab;

        private PlayerModel _player;

        private IUserKeyProxy _modificationItemMenuInputProxy;
        private bool _modificationItemMenuInput;
        private bool _weaponNull;

        private static readonly Vector3 MoveVector = new Vector3(-0.5f, 0.3f, -0.1f);
        private static readonly Vector3 RotateVector = new Vector3(-25f, -50f, -20f);

        private void OnModificationItemMenuInput(bool value) => _modificationItemMenuInput = value;

        public WeaponHudController(PlayerInitialization playerInitialization, DataStore data)
        {
            _playerInitialization = playerInitialization;
            _iconPrefab = data.IconPrefab;

            _weaponNull = true;
            _modificators = new Dictionary<int, IWeaponModificatorData>(6);
            _buttons = new Dictionary<int, Button>(6);

            _modificationItemMenuInputProxy = KeysInput.ModificationItemMenu;
        }
        
        public void Initialization()
        {
            _playerHudView = _playerInitialization.GetPlayerHud();
            _player = _playerInitialization.GetPlayer();

            _modificationItemMenuInputProxy.KeyOnChange += OnModificationItemMenuInput;
        }
        
        public void Execute(float deltaTime)
        {
            if (_player.Weapon != null && _weaponNull)
                GenerateHud();
            else if (_player.Weapon == null && !_weaponNull)
                ClearHud();

            var menu = _playerHudView.ModificatorMenu;
            if (_player.View == null || _player.Weapon == null)
                return;

            var weaponTransform = _player.Weapon.Transform;
            if (_modificationItemMenuInput && !menu.activeSelf && !_player.Weapon.Blocking)
            {
                _playerHudView.ModificatorMenu.SetActive(true);
                weaponTransform.DOLocalMove(MoveVector, 0.2f);
                weaponTransform.DOLocalRotate(RotateVector, 0.2f);
                _player.Weapon.Blocking = true;

                _player.CanMove = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if (!_modificationItemMenuInput && menu.activeSelf)
            {
                _playerHudView.ModificatorMenu.SetActive(false);
                weaponTransform.DOLocalMove(weaponTransform.localPosition - MoveVector, 0.2f);
                weaponTransform.DOLocalRotate(weaponTransform.localRotation.eulerAngles - RotateVector, 0.2f);
                _player.Weapon.Blocking = false;
                
                _player.CanMove = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OnModificatorClick(BarrelModificatorData modificatorData)
        {
            var weapon = _player.Weapon;
            _player.Weapon.ShootProxy.WeaponModification?.RemoveModification();
            weapon.SetShootProxy(weapon.ShootDefaultProxy);

            if (modificatorData.ModificatorPrefab == null)
                return;
            
            var modificationBarrel = new BarrelModification(modificatorData, weapon.View.BarrelPosition);
            modificationBarrel.ApplyModification(weapon);
            weapon.SetShootProxy(modificationBarrel);
        }
        
        private void OnModificatorClick(AimModificatorData modificatorData)
        {
            var weapon = _player.Weapon;
            _player.Weapon.AimProxy.WeaponModification?.RemoveModification();
            weapon.SetAimProxy(weapon.AimDefaultProxy);
            
            if (modificatorData.ModificatorPrefab == null)
                return;

            var modificationAim = new AimModification(modificatorData, weapon.View.AimPosition);
            modificationAim.ApplyModification(weapon);
            weapon.SetAimProxy(modificationAim);
        }

        private void GenerateHud()
        {
            var weapon = _player.Weapon;
            
            _barrelModificators = weapon.Data.BarrelModifications;
            _aimModificators = weapon.Data.AimModifications;

            // TODO: Не оптимизированно! Лучше менять Enabled вместо создания и удаления объектов. ПЕРЕДЕЛАТЬ!
            for (var index = 0; index < _barrelModificators.Length; index++)
            {
                var modificator = _barrelModificators[index];
                var button = ModificatorInit(modificator, _playerHudView.BarrelContent);
                button.onClick.AddListener(delegate { OnModificatorClick(modificator); });
            }

            for (var index = 0; index < _aimModificators.Length; index++)
            {
                var modificator = _aimModificators[index];
                var button = ModificatorInit(modificator, _playerHudView.AimContent);
                button.onClick.AddListener(delegate { OnModificatorClick(modificator); });
            }

            _weaponNull = false;
        }

        private void ClearHud()
        {
            foreach (var button in _buttons)
            {
                // TODO: Не оптимизированно! Лучше менять Enabled вместо создания и удаления объектов. ПЕРЕДЕЛАТЬ!
                button.Value.onClick.RemoveAllListeners();
                Object.Destroy(button.Value.gameObject);
            }
                
            _modificators = new Dictionary<int, IWeaponModificatorData>(6);
            _buttons = new Dictionary<int, Button>(6);
                
            _weaponNull = true;
        }

        private Button ModificatorInit(IWeaponModificatorData modificator, Transform placePoint)
        {
            var gameObject = Object.Instantiate(_iconPrefab, placePoint);
            if (!gameObject.TryGetComponent(out Button button))
                throw new Exception("Button отсуствует у IconPrefab");
                    
            button.image.sprite = modificator.Icon;
            var gameObjectID = gameObject.GetInstanceID();
                
            _buttons.Add(gameObjectID, button);
            _modificators.Add(gameObjectID, modificator);
            return button;
        }

        public void Cleanup()
        {
            _modificationItemMenuInputProxy.KeyOnChange -= OnModificationItemMenuInput;

            foreach (var button in _buttons)
            {
                button.Value.onClick.RemoveAllListeners();
            }
        }
    }
}