using Code.Data;
using Code.Interfaces.Models;
using Code.Views;
using UnityEngine;

namespace Code.Models
{
    internal sealed class PlayerModel: IPlayerModel, IPlayerMovementModel
    {
        public int Score { get; set; }
        public float Health { get; set; }
        public float Armor { get; set; }
        
        public float WalkingSpeed { get; set; }
        public float RunningSpeed { get; set; }
        public float JumpForce { get; set; }
        
        public Vector3 SpawnPointPosition { get; set; }
        public Vector3 SpawnPointRotation { get; set; }
        
        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }
        public InteractView ObjectInHand { get; private set; }

        public Camera Camera { get; }
        public PlayerView View { get; }
        public PlayerData Data { get; }
        
        public WeaponModel Weapon { get; set; }
        public WeaponView DefaultWeapon { get; set; }

        public bool CanMove { get; set; }
        public Transform CameraTransform { get; }
        
        public CharacterController CharacterController { get; }
        public AudioSource AudioSource { get; }
        
        public PlayerModel(PlayerView view, PlayerData data, AudioSource audioSource, CharacterController characterController, Camera camera, WeaponModel weapon = null)
        {
            View = view;
            Data = data;
            Camera = camera;
            
            Weapon = weapon;
            
            Reset();
            ResetMovement();

            Transform = view.transform;
            GameObject = view.gameObject;
            
            CameraTransform = camera.transform;
            CharacterController = characterController;
            AudioSource = audioSource;
        }
        
        public void Reset()
        {
            Health = Data.MaxHealth;
            Armor = Data.MaxArmor;
        }
        
        public void ResetMovement()
        {
            CanMove = true;
            WalkingSpeed = Data.WalkingSpeed;
            RunningSpeed = Data.RunningSpeed;
            JumpForce = Data.JumpForce;
        }
        
        public void SetObjectInHand(InteractView item)
        {
            ObjectInHand = item;
        }

    }
}