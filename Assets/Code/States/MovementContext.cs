using Code.Models;
using UnityEngine;

namespace Code.States
{
    internal sealed class MovementContext
    {
        private MovementState _movementState;

        public MovementContext(MovementState movementState, PlayerModel player)
        {
            _movementState = movementState;
            Player = player;
        }

        public MovementState MovementState { set => _movementState = value; }
        
        public float DeltaTime { get; private set; }
        public bool RunInput { get; private set; }
        public bool JumpInput { get; private set; }
        
        public Vector3 MovementInput { get; private set; }
        public Vector3 MovementDirection { get; set; }

        public PlayerModel Player { get; }

        public void Update(float deltaTime, Vector3 movementInput, bool runInput, bool jumpInput)
        {
            RunInput = runInput;
            JumpInput = jumpInput;
            MovementInput = movementInput;
            DeltaTime = deltaTime;
        }

        public void Request()
        {
            _movementState.Handle(this);
        }
    }
}