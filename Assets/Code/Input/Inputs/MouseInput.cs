using Code.Interfaces.Input;

namespace Code.Input.Inputs
{
    internal sealed class MouseInput
    {
        public static IUserKeyProxy Fire { get; private set; }
        public static IUserAxisProxy MouseX { get; private set; }
        public static IUserAxisProxy MouseY { get; private set; }

        public MouseInput(IUserKeyProxy fire, IUserAxisProxy mouseX, IUserAxisProxy mouseY)
        {
            Fire = fire;
            MouseX = mouseX;
            MouseY = mouseY;
        }
    }
}