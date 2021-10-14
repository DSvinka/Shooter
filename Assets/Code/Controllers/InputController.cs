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
            MouseInput.Aim.GetKey();
            
            KeysInput.Escape.GetKeyDown();
            KeysInput.Reload.GetKeyDown();
            KeysInput.Interact.GetKeyDown();
            KeysInput.SaveGame.GetKeyDown();
            KeysInput.ModificationItemMenu.GetKey();
            KeysInput.Drop.GetKeyDown();
            KeysInput.Jump.GetKeyDown();
            KeysInput.Run.GetKey();

            
            
        }
    }
}