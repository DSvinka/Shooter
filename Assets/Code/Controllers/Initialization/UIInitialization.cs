using Code.Factory;
using Code.Interfaces;
using Code.Views;

namespace Code.Controllers.Initialization
{
    internal sealed class UIInitialization: IInitialization
    {
        private readonly UIFactory _uiFactory;

        private HudView _hudView;
        private EscapeMenuView _escapeMenuView;

        public UIInitialization(UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        
        public void Initialization()
        {
            _hudView = _uiFactory.CreateHud();
            _hudView.gameObject.SetActive(true);
            
            _escapeMenuView = _uiFactory.CreateEscapeMenu();
            _escapeMenuView.gameObject.SetActive(false);
            _escapeMenuView.transform.SetParent(null);
        }

        public HudView GetHud()
        {
            return _hudView;
        }
        
        public EscapeMenuView GetEscapeMenu()
        {
            return _escapeMenuView;
        }
    }
}