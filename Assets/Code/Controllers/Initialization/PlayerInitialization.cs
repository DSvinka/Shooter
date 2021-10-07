using System;
using Code.Data;
using Code.Factory;
using Code.Interfaces;
using Code.Models;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class PlayerInitialization: IInitialization
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Vector3 _playerSpawnPosition;

        private PlayerData _data;
        private PlayerModel _player;
        private PlayerHudView _playerHud;

        public PlayerInitialization(PlayerData data, PlayerFactory playerFactory, Vector3 playerSpawnPosition)
        {
            _data = data;
            _playerFactory = playerFactory;
            _playerSpawnPosition = playerSpawnPosition;
        }

        public void Initialization()
        {
            var playerView = _playerFactory.CreatePlayer();
            var playerCamera = playerView.GetComponentInChildren<Camera>();
            if (playerCamera == null)
                throw new Exception("Компонент Camera не найден в детях объекта PlayerView");
            
            var playerCharacterController = playerView.GetComponent<CharacterController>();
            if (playerCharacterController == null)
                throw new Exception("Компонент CharacterController не найден на объекте PlayerView");
            
            var playerModel = new PlayerModel(playerView, _data, playerCharacterController, playerCamera,  null);

            _player = playerModel;
            _playerHud = _playerFactory.CreatePlayerHud();

            _playerHud.transform.SetParent(null);
            _player.Transform.SetParent(null);
            _player.Transform.position = _playerSpawnPosition;
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