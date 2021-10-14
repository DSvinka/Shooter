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
            if (!gameObject.TryGetComponent(out PlayerView view))
                throw new Exception("У префаба игрока не найден компонент PlayerView!");
            return view;
        }

        public PlayerHudView CreatePlayerHud()
        {
            var gameObject = Object.Instantiate(_data.PlayerHudPrefab);
            if (!gameObject.TryGetComponent(out PlayerHudView view))
                throw new Exception("У префаба игрока не найден компонент PlayerHudView!");
            return view;
        }
    }
}