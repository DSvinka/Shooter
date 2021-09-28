using System;
using Code.Interfaces.Input;
using Code.Managers;

namespace Code.Input
{
    public sealed class MouseX : IUserAxisProxy
    {
        public event Action<float> AxisOnChange = delegate(float f) {  };

        public void GetAxis()
        {
            AxisOnChange.Invoke(UnityEngine.Input.GetAxis(MouseManager.MOUSEX));
        }
    }
}