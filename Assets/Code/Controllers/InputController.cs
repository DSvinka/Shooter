using Code.Input.Inputs;
using Code.Interfaces;

namespace Code.Controllers
{
    internal sealed class InputController : IExecute
    {
        public void Execute(float deltaTime)
        {
            AxisInput.Horizontal.GetAxis();
            AxisInput.Vertical.GetAxis();
            
            MouseInput.MouseY.GetAxis();
            MouseInput.MouseX.GetAxis();
            MouseInput.Fire.GetKey();
            
            KeysInput.Escape.GetKeyDown();
            KeysInput.Reload.GetKeyDown();
            KeysInput.Jump.GetKeyDown();
            KeysInput.Run.GetKey();

            
            
        }
    }
}