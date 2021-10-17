using UnityEngine;

namespace Code.Interfaces.Models
{
    public interface IPlayerMovementModel
    {
        float WalkingSpeed { get; set; }
        float RunningSpeed { get; set; }
        float JumpForce { get; set; }
        
        bool CanMove { get; set; }
        Transform CameraTransform { get; }
        CharacterController CharacterController { get; }

        void ResetMovement();
    }
}