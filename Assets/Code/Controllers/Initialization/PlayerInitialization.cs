using System;
using Code.Data;
using Code.Factory;
using Code.Models;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class PlayerInitialization
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Transform _playerSpawnPoint;

        private PlayerData _data;
        private PlayerModel _player;

        public PlayerInitialization(PlayerData data, PlayerFactory playerFactory, Transform playerSpawnPoint)
        {
            _data = data;
            _playerFactory = playerFactory;
            _playerSpawnPoint = playerSpawnPoint;
        }

        public PlayerModel Initialization(bool saveLoaded)
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
            
            _player.Transform.SetParent(null);
            if (!saveLoaded)
            {
                _player.Transform.position = _playerSpawnPoint.position;
                _player.Transform.rotation = _playerSpawnPoint.rotation;
            }

            return _player;
        }

        public PlayerModel GetPlayer()
        {
            return _player;
        }
    }
}