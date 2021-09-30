using System;
using Code.Factory;
using Code.Interfaces;
using Code.Views;
using UnityEngine;

namespace Code.Controllers.Initialization
{
    internal sealed class PlayerInitialization: IInitialization
    {
        private readonly PlayerFactory _playerFactory;
        private readonly Vector3 _playerSpawnPosition;
        
        private PlayerView _player;
        private PlayerHudView _playerHud;

        public Camera Camera { get; private set; }

        public PlayerInitialization(PlayerFactory playerFactory, Vector3 playerSpawnPosition)
        {
            _playerFactory = playerFactory;
            _playerSpawnPosition = playerSpawnPosition;
        }

        public void Initialization()
        {
            _player = _playerFactory.CreatePlayer();
            if (_player == null)
                throw new Exception("Компонент Player остуствует у префаба игрока");
            
            _playerHud = _playerFactory.CreatePlayerHud();
            if (_playerHud == null)
                throw new Exception("Компонент PlayerHud остуствует у префаба интерфейса игрока");
            
            Camera = _player.GetComponentInChildren<Camera>();

            _playerHud.transform.SetParent(null);
            _player.transform.SetParent(null);
            _player.transform.position = _playerSpawnPosition;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public PlayerView GetPlayer()
        {
            return _player;
        }

        public PlayerHudView GetPlayerHud()
        {
            return _playerHud;
        }
    }
}