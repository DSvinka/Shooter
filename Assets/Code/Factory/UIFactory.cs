using System;
using Code.Data.DataStores;
using Code.Interfaces.Factory;
using Code.Views;
using Object = UnityEngine.Object;

namespace Code.Factory
{
    internal sealed class UIFactory: IFactory, IUIFactory
    {
        private readonly UIStore _uiStore;
        
        public UIFactory(UIStore uiStore)
        {
            _uiStore = uiStore;
        }
        
        public HudView CreateHud()
        {
            var gameObject = Object.Instantiate(_uiStore.HudPrefab);
            if (!gameObject.TryGetComponent(out HudView view))
                throw new Exception("У префаба HUD'a игрока не найден компонент PlayerHudView!");
            return view;
        }

        public EscapeMenuView CreateEscapeMenu()
        {
            var gameObject = Object.Instantiate(_uiStore.EscapeMenuPrefab);
            if (!gameObject.TryGetComponent(out EscapeMenuView view))
                throw new Exception("У префаба Escape Menu не найден компонент EscapeMenuView!");
            return view;
        }
    }
}