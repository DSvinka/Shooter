using Code.Views;

namespace Code.Interfaces.Factory
{
    internal interface IUIFactory
    {
        HudView CreateHud();
        EscapeMenuView CreateEscapeMenu();
    }
}