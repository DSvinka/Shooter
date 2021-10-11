using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Managers;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class PickupController: IController, IInitialization, IExecute, ICleanup
    {
        private readonly Dictionary<int, InteractView> _pickupViews;
        private readonly WeaponController _weaponController;
        private readonly PlayerInitialization _playerInitialization;

        private PlayerModel _player;

        private bool _interactInput;
        private IUserKeyDownProxy _interactInputProxy;
        
        public PickupController(WeaponController weaponController, PlayerInitialization playerInitialization, Dictionary<int, InteractView> pickupViews)
        {
            _pickupViews = pickupViews;
            _weaponController = weaponController;
            _playerInitialization = playerInitialization;
            
            _interactInputProxy = KeysInput.Interact;
        }
        
        public void Initialization()
        {
            _player = _playerInitialization.GetPlayer();
            _interactInputProxy.KeyOnDown += OnInteractInput;
            
            foreach (var pickupView in _pickupViews)
            {
                pickupView.Value.OnInteract += OnInteract;
            }
        }

        public void Execute(float deltaTime)
        {
            if (_interactInput)
            {
                var ray = _player.Camera.ViewportPointToRay(VectorManager.ScreenCenter);
                if (Physics.Raycast(ray, out var hit, _player.Data.MaxInteractDistance))
                {
                    var interactObject = hit.collider.gameObject.GetComponent<InteractView>();
                    if (interactObject != null)
                    {
                        interactObject.Interact(_player.GameObject.GetInstanceID());
                    }
                }
            }
        }
        
        private void OnInteractInput(bool value) => _interactInput = value;

        private void OnInteract(GameObject item, int viewID, int unitID)
        {
            var pickupView = _pickupViews[viewID];
            var gameObject = pickupView.gameObject;

            if (!gameObject.activeSelf)
                return;
            
            var weaponView = item.GetComponent<WeaponView>();
            if (weaponView == null)
                throw new Exception("WeaponView отсуствует в Item");

            _weaponController.ChangeWeapon(weaponView.WeaponType);
            
            var boxColliders = gameObject.GetComponents<BoxCollider>();
            if (boxColliders.Length != 0)
            {
                foreach (var boxCollider in boxColliders)
                {
                    boxCollider.enabled = false;
                }
            }

            gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _interactInputProxy.KeyOnDown -= OnInteractInput;
            foreach (var pickupView in _pickupViews)
            {
                pickupView.Value.OnInteract -= OnInteract;
            }
        }
    }
}