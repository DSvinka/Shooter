using UnityEngine;

namespace Code.States.Player
{
    internal sealed class MovementRun: MovementState
    {
        private MovementState _walkState;
        
        public override void Handle(MovementContext context)
        {
            var characterController = context.Player.CharacterController;
            var transform = context.Player.Transform;
            var canMove = context.Player.CanMove;
            var data = context.Player.Data;
        
            var forward = transform.TransformDirection(Vector3.forward);
            var right = transform.TransformDirection(Vector3.right);
        
            var curSpeedX = canMove ? data.RunningSpeed * context.MovementInput.y : 0;
            var curSpeedY = canMove ? data.RunningSpeed * context.MovementInput.x : 0;

            var direction = (forward * curSpeedX) + (right * curSpeedY);
            direction.y = context.MovementDirection.y;

            characterController.Move(direction * context.DeltaTime);
            context.MovementDirection = direction;
            
            if (context.RunInput)
                return;

            if (_walkState == null)
                _walkState = new MovementWalk();
            
            context.MovementState = _walkState;
        }
    }
}