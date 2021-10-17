using System;
using Code.Data.DataStores;
using Code.Interfaces.Factory;
using Code.Views;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class PlayerFactory: IFactory, IPlayerFactory
    {
        private readonly UnitStore _unitStore;

        public PlayerFactory(UnitStore unitStore, UIStore uiStore)
        {
            _unitStore = unitStore;
        }

        public PlayerView CreatePlayer()
        {
            var gameObject = Object.Instantiate(_unitStore.PlayerData.PlayerPrefab);
            if (!gameObject.TryGetComponent(out PlayerView view))
                throw new Exception("У префаба игрока не найден компонент PlayerView!");
            return view;
        }

        
    }
}