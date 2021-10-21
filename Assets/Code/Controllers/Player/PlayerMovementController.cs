using Code.Controllers.Initialization;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Models;
using Code.States;
using Code.States.Player;
using UnityEngine;

namespace Code.Controllers.Player
{
    internal sealed class PlayerMovementController: IController, IExecute, IInitialization, ICleanup
    {
        private readonly PlayerInitialization _playerInitialization;
        private PlayerModel _player;
        private MovementContext _movementContext;
        
        private float _rotationX;

        private Vector2 _movementInput;
        private Vector2 _mouseInput;
        private bool _jumpInput;
        private bool _runInput;

        public PlayerMovementController(PlayerInitialization playerInitialization)
        {
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
            _movementContext = new MovementContext(new MovementWalk(), _player);

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
            var data = _player.Data;

            _movementContext.Update(deltaTime, _movementInput, _runInput, _jumpInput);
            _movementContext.Request();

            if (!_player.CharacterController.isGrounded)
            {
                var moveDirection = _movementContext.MovementDirection;
                moveDirection.y -= data.JumpForce * deltaTime;
                _movementContext.MovementDirection = moveDirection;
            }

            if (_player.CanMove)
            {
                _rotationX += -_mouseInput.y * data.LookSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -data.LookXLimit, data.LookXLimit);
                _player.CameraTransform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                _player.Transform.rotation *= Quaternion.Euler(0, _mouseInput.x * data.LookSpeed, 0);
            }
        }
    }
}