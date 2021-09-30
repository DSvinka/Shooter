using System;
using Code.Data.DataStores;
using Code.Interfaces.Factory;
using Code.Views;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class PlayerFactory: IFactory, IPlayerFactory
    {
        private DataStore _data;
        
        public PlayerFactory(DataStore data)
        {
            _data = data;
        }

        public PlayerView CreatePlayer()
        {
            var gameObject = Object.Instantiate(_data.PlayerData.PlayerPrefab);
            var player = gameObject.GetComponent<PlayerView>();
            if (player == null)
                throw new Exception("У префаба игрока не найден компонент PlayerView!");
            return player;
        }

        public PlayerHudView CreatePlayerHud()
        {
            var gameObject = Object.Instantiate(_data.PlayerHudPrefab);
            var playerHud = gameObject.GetComponent<PlayerHudView>();
            if (playerHud == null)
                throw new Exception("У префаба игрока не найден компонент PlayerHudView!");
            return playerHud;
        }
    }
}