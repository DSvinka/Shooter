using Code.Models;
using UnityEngine;

namespace Code.States.Player
{
    internal sealed class MovementWalk: MovementState
    {
        private MovementState _runState;
        private MovementState _jumpState;

        public override void Handle(MovementContext context)
        {
            var characterController = context.Player.CharacterController;
            var transform = context.Player.Transform;
            var canMove = context.Player.CanMove;
            var data = context.Player.Data;
        
            var forward = transform.TransformDirection(Vector3.forward);
            var right = transform.TransformDirection(Vector3.right);
        
            var curSpeedX = canMove ? data.WalkingSpeed * context.MovementInput.y : 0;
            var curSpeedY = canMove ? data.WalkingSpeed * context.MovementInput.x : 0;
            
            var direction = (forward * curSpeedX) + (right * curSpeedY);
            direction.y = context.MovementDirection.y;

            characterController.Move(direction * context.DeltaTime);
            context.MovementDirection = direction;
            
            if (context.RunInput)
            {
                if (_runState == null)
                    _runState = new MovementRun();
            
                context.MovementState = _runState;
            }
            
            else if (context.JumpInput)
            {
                if (_jumpState == null)
                    _jumpState = new MovementJump();
            
                context.MovementState = _jumpState;
            }
        }
    }
}