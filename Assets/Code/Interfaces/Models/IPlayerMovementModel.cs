using UnityEngine;

namespace Code.Interfaces.Models
{
    public interface IPlayerMovementModel
    {
        bool CanMove { get; set; }
        Transform CameraTransform { get; }
        CharacterController CharacterController { get; }
    }
}