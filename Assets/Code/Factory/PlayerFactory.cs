using Code.Data.DataStores;
using Code.Interfaces;
using Code.Interfaces.Factory;
using UnityEngine;

namespace Code.Factory
{
    internal sealed class PlayerFactory: IFactory, IPlayerFactory
    {
        private DataStore _data;
        
        public PlayerFactory(DataStore data)
        {
            _data = data;
        }

        public Transform CreatePlayer()
        {
            var player = Object.Instantiate(_data.PlayerData.PlayerPrefab);
            return player.transform;
        }

        public Transform CreatePlayerHud()
        {
            var player = Object.Instantiate(_data.PlayerHudPrefab);
            return player.transform;
        }
    }
}