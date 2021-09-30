using Code.Interfaces.Input;

namespace Code.Input.Inputs
{
    internal sealed class MouseInput
    {
        public static IUserKeyProxy Fire { get; private set; }
        public static IUserKeyProxy Aim { get; private set; }
        public static IUserAxisProxy MouseX { get; private set; }
        public static IUserAxisProxy MouseY { get; private set; }

        public MouseInput(IUserKeyProxy fire, IUserKeyProxy aim,  IUserAxisProxy mouseX, IUserAxisProxy mouseY)
        {
            Fire = fire;
            Aim = aim; 
            MouseX = mouseX;
            MouseY = mouseY;
        }
    }
}