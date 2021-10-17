using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Managers;
using Code.Models;
using Code.Utils.Extensions;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Player
{
    internal sealed class InteractController: IController, IInitialization, IExecute, ICleanup
    {
        private readonly Dictionary<int, InteractView> _pickupViews;
        private readonly WeaponController _weaponController;
        private readonly PlayerInitialization _playerInitialization;

        private PlayerModel _player;

        private bool _interactInput;
        private bool _dropInput;
        
        private IUserKeyDownProxy _interactInputProxy;
        private IUserKeyDownProxy _dropInputProxy;
        
        public InteractController(WeaponController weaponController, PlayerInitialization playerInitialization, Dictionary<int, InteractView> pickupViews)
        {
            _pickupViews = pickupViews;
            _weaponController = weaponController;
            _playerInitialization = playerInitialization;
            
            _interactInputProxy = KeysInput.Interact;
            _dropInputProxy = KeysInput.Drop;
        }
        
        private void OnInteractInput(bool value) => _interactInput = value;
        private void OnDropInput(bool value) => _dropInput = value;
        
        public void Initialization()
        {
            _player = _playerInitialization.GetPlayer();
            
            _interactInputProxy.KeyOnDown += OnInteractInput;
            _dropInputProxy.KeyOnDown += OnDropInput;
            
            foreach (var pickupView in _pickupViews)
            {
                pickupView.Value.OnInteract += OnInteract;
            }
        }

        public void Execute(float deltaTime)
        {
            if (_interactInput)
                Interact();
            
            if (_dropInput)
                Drop();
        }
        
        private void Interact()
        {
            var ray = _player.Camera.ViewportPointToRay(VectorManager.ScreenCenter);
            if (Physics.Raycast(ray, out var hit, _player.Data.MaxInteractDistance)) 
            {
                if (hit.collider.gameObject.TryGetComponent(out InteractView view))
                {
                    view.Interact(_player.GameObject.GetInstanceID());
                }
            }
        }
        private void Drop()
        {
            if (_player.ObjectInHand == null)
                return;
            
            var interactView = _player.ObjectInHand;
            var gameObject = _player.ObjectInHand.gameObject;
            var transform = _player.ObjectInHand.transform;

            transform.parent = null;
            _player.SetObjectInHand(null);
            
            if (interactView.Item.TryGetComponent(out WeaponView weaponView))
            {
                _player.Weapon = null;
            }

            if (interactView.Item.TryGetComponent(out AudioSource audioSource))
                audioSource.Stop();
            
            var colliders = gameObject.GetComponents<Collider>();
            if (colliders.Length != 0)
            {
                foreach (var collider in colliders)
                {
                    collider.enabled = true;
                }
            }

            if (gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.AddForce(transform.forward * _player.Data.DropItemForce, ForceMode.Acceleration);
            }
        }

        private void OnInteract(GameObject item, int viewID, int unitID)
        {
            var interactView = _pickupViews[viewID];
            var interactType = interactView.InteractType;

            var gameObject = interactView.gameObject;
            var transform = interactView.transform;

            bool disableCollision;

            if (_player.ObjectInHand != null)
                return;

            switch (interactType)
            {
                case InteractManager.InteractType.Weapon:
                    if (!item.TryGetComponent(out WeaponView weaponView))
                        throw new Exception("WeaponView отсуствует в Item");

                    _weaponController.ChangeWeapon(weaponView);
                    
                    disableCollision = true;
                    _player.SetObjectInHand(interactView);

                    break;
                
                case InteractManager.InteractType.PhysicItem:
                    transform.parent = _player.View.HandPoint;
                    
                    disableCollision = false;
                    _player.SetObjectInHand(interactView);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactType), "Указанный тип интерактивного объекта не предусмотрен в коде");
            }
            
            transform.localRotation = _player.View.HandPoint.localRotation;
            gameObject.DisableAllPhysics(disableCollision);
            if (disableCollision)
                gameObject.DisableAllCollision();
        }

        public void Cleanup()
        {
            _interactInputProxy.KeyOnDown -= OnInteractInput;
            _dropInputProxy.KeyOnDown -= OnDropInput;
            
            foreach (var pickupView in _pickupViews)
            {
                pickupView.Value.OnInteract -= OnInteract;
            }
        }
    }
}