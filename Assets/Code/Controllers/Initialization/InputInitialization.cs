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
            var inputInteract = new InputInteract();
            var inputJump = new InputJump();
            var inputRun = new InputRun();
            var keysInput = new KeysInput(inputEscape, inputReload, inputInteract, inputJump, inputRun);
            
            var inputFire = new InputFireMouse();
            var inputAim = new InputAimMouse();
            var mouseX = new MouseX();
            var mouseY = new MouseY();
            var mouseInput = new MouseInput(inputFire, inputAim, mouseX, mouseY);
        }
    }
}