using System;
using Code.Controllers.Initialization;
using Code.Data;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Views;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class PlayerMovementController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerData _data;
        private readonly PlayerInitialization _playerInitialization;
        
        private PlayerView _player;
        private Camera _camera;
        private CharacterController _characterController;

        private Vector3 _moveDirection;
        private float _rotationX;

        private Vector2 _movementInput;
        private Vector2 _mouseInput;
        private bool _jumpInput;
        private bool _runInput;
        
        public bool canMove = true;

        public PlayerMovementController(PlayerData data, PlayerInitialization playerInitialization)
        {
            _data = data;
            _playerInitialization = playerInitialization;
            
            _movementXInputProxy = AxisInput.Horizontal;
            _movementYInputProxy = AxisInput.Vertical;
            _mouseXProxy = MouseInput.MouseX;
            _mouseYProxy = MouseInput.MouseY;
            _jumpInputProxy = KeysInput.Jump;
            _runInputProxy = KeysInput.Run;
        }

        #region Input Proxy

        private IUserAxisProxy _movementXInputProxy;
        private IUserAxisProxy _movementYInputProxy;
        private IUserAxisProxy _mouseXProxy;
        private IUserAxisProxy _mouseYProxy;
        private IUserKeyDownProxy _jumpInputProxy;
        private IUserKeyProxy _runInputProxy;
        
        private void OnMovementVerticalInput(float value) => _movementInput.y = value;
        private void OnMovementHorizontalInput(float value) => _movementInput.x = value;
        private void OnMouseXInput(float value) => _mouseInput.x = value;
        private void OnMouseYInput(float value) => _mouseInput.y = value;
        private void OnJumpInput(bool value) => _jumpInput = value;
        private void OnRunInput(bool value) => _runInput = value;

        #endregion

        public void Initialization()
        {
            _player = _playerInitialization.GetPlayer();
            
            _characterController = _player.GetComponent<CharacterController>();
            if (_characterController == null)
                throw new Exception("Компонент CharacterController остуствует на префабе игрока");
            _camera = _player.GetComponentInChildren<Camera>();
            if (_camera == null)
                throw new Exception("Объект Camera остуствует на префабе игрока");

            _mouseXProxy.AxisOnChange += OnMouseXInput;
            _mouseYProxy.AxisOnChange += OnMouseYInput;
            
            _movementXInputProxy.AxisOnChange += OnMovementHorizontalInput;
            _movementYInputProxy.AxisOnChange += OnMovementVerticalInput;

            _jumpInputProxy.KeyOnDown += OnJumpInput;
            _runInputProxy.KeyOnChange += OnRunInput;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Cleanup()
        {
            _mouseXProxy.AxisOnChange -= OnMouseXInput;
            _mouseYProxy.AxisOnChange -= OnMouseYInput;
            
            _movementXInputProxy.AxisOnChange -= OnMovementHorizontalInput;
            _movementYInputProxy.AxisOnChange -= OnMovementVerticalInput;

            _jumpInputProxy.KeyOnDown -= OnJumpInput;
            _runInputProxy.KeyOnChange -= OnRunInput;
        }

        public void Execute(float deltaTime)
        {
            var forward = _player.transform.TransformDirection(Vector3.forward);
            var right = _player.transform.TransformDirection(Vector3.right);

            var isRunning = _runInput;
            var curSpeedX = canMove ? (isRunning ? _data.RunningSpeed : _data.WalkingSpeed) * _movementInput.y : 0;
            var curSpeedY = canMove ? (isRunning ? _data.RunningSpeed : _data.WalkingSpeed) * _movementInput.x : 0;
            var movementDirectionY = _moveDirection.y;
            _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (_jumpInput && canMove && _characterController.isGrounded)
            {
                _moveDirection.y = _data.JumpForce;
            }
            else
            {
                _moveDirection.y = movementDirectionY;
            }
            
            if (!_characterController.isGrounded)
            {
                _moveDirection.y -= _data.JumpForce * deltaTime;
            }
            
            _characterController.Move(_moveDirection * deltaTime);
            
            if (canMove)
            {
                _rotationX += -_mouseInput.y * _data.LookSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -_data.LookXLimit, _data.LookXLimit);
                _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                _player.transform.rotation *= Quaternion.Euler(0, _mouseInput.x * _data.LookSpeed, 0);
            }
        }
    }
}