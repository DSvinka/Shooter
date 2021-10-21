using UnityEngine;

namespace Code.States.Player
{
    internal sealed class MovementJump: MovementState
    {
        private MovementState _nextMovementState;
    
        public override void Handle(MovementContext context)
        {
            var characterController = context.Player.CharacterController;
            var canMove = context.Player.CanMove;
            var data = context.Player.Data;

            if (canMove && characterController.isGrounded)
            {
                var direction = context.MovementDirection;
                direction.y = data.JumpForce;
                context.MovementDirection = direction;
            }
            
            if (context.RunInput)
                return;

            if (_nextMovementState == null)
                _nextMovementState = new MovementWalk();
        
            context.MovementState = _nextMovementState;
        }
    }
}