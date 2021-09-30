using System;
using Code.Interfaces.Input;
using Code.Managers;

namespace Code.Input
{
    internal sealed class InputAimMouse: IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };

        public void GetKey()
        {
            KeyOnChange.Invoke(UnityEngine.Input.GetMouseButton(MouseManager.AIM));
        }
    }
}