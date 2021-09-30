using Code.Input;
using Code.Input.Inputs;

namespace Code.Controllers.Initialization
{
    internal sealed class InputInitialization
    {
        public InputInitialization()
        {
            var axisHorizontal = new AxisHorizontal();
            var axisVertical = new AxisVertical();
            var axisInput = new AxisInput(axisHorizontal, axisVertical);
            
            var inputEscape = new InputEscape();
            var inputReload = new InputReload();
            var inputJump = new InputJump();
            var inputRun = new InputRun();
            var keysInput = new KeysInput(inputEscape, inputReload, inputJump, inputRun);
            
            var inputFire = new InputFireMouse();
            var mouseX = new MouseX();
            var mouseY = new MouseY();
            var mouseInput = new MouseInput(inputFire, mouseX, mouseY);
        }
    }
}