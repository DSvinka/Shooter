using System;
using Code.Data;
using Code.Factory;
using Code.Interfaces;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class PlayerInitialization
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Transform _playerSpawnPoint;

        private PlayerData _data;
        private PlayerModel _player;
        private PlayerHudView _playerHud;

        public PlayerInitialization(PlayerData data, PlayerFactory playerFactory, Transform playerSpawnPoint)
        {
            _data = data;
            _playerFactory = playerFactory;
            _playerSpawnPoint = playerSpawnPoint;
        }

        public PlayerModel Initialization()
        {
            var view = _playerFactory.CreatePlayer();
            var camera = view.GetComponentInChildren<Camera>();
            if (camera == null)
                throw new Exception("Компонент Camera не найден в детях объекта PlayerView");
            
            if (!view.TryGetComponent(out AudioSource audioSource))
                throw new Exception("Компонент AudioSource не найден на объекте PlayerView");
            
            if (!view.TryGetComponent(out CharacterController characterController))
                throw new Exception("Компонент CharacterController не найден на объекте PlayerView");
            
            var playerModel = new PlayerModel(view, _data, audioSource, characterController, camera,  null)
            {
                SpawnPointPosition = _playerSpawnPoint.position,
                SpawnPointRotation = _playerSpawnPoint.rotation.eulerAngles,
            };

            _player = playerModel;
            _playerHud = _playerFactory.CreatePlayerHud();

            _playerHud.transform.SetParent(null);
            _player.Transform.SetParent(null);
            _player.Transform.position = _playerSpawnPoint.position;
            _player.Transform.rotation = _playerSpawnPoint.rotation;

            return _player;
        }

        public PlayerModel GetPlayer()
        {
            return _player;
        }

        public PlayerHudView GetPlayerHud()
        {
            return _playerHud;
        }
    }
}