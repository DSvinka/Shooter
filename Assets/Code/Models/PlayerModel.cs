﻿using Code.Data;
using Code.Interfaces.Models;
using Code.Views;
using UnityEngine;

namespace Code.Models
{
    internal sealed class PlayerModel: IUnitModel, IPlayerModel, IPlayerMovementModel
    {
        public float Health { get; set; }
        public float Armor { get; set; }

        public Transform Transform { get; set; }
        public GameObject GameObject { get; set; }

        public Camera Camera { get; }
        public PlayerView View { get; }
        public PlayerData Data { get; }
        
        public WeaponModel Weapon { get; set; }
        
        public bool CanMove { get; set; }
        public Transform CameraTransform { get; }
        public CharacterController CharacterController { get; }

        public PlayerModel(PlayerView view, PlayerData data, CharacterController characterController, Camera camera, WeaponModel weapon = null)
        {
            View = view;
            Data = data;
            Camera = camera;
            
            Weapon = weapon;
            Health = data.MaxHealth;
            Armor = data.MaxArmor;

            Transform = view.transform;
            GameObject = view.gameObject;

            CanMove = true;
            CameraTransform = camera.transform;
            CharacterController = characterController;
        }

       
    }
}