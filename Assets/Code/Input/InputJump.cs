using System;
using Code.Interfaces.Input;
using Code.Managers;

namespace Code.Input
{
    internal sealed class InputJump : IUserKeyDownProxy
    {
        public event Action<bool> KeyOnDown = delegate(bool f) { };

        public void GetKeyDown()
        {
            KeyOnDown.Invoke(UnityEngine.Input.GetKeyDown(KeysManager.JUMP));
        }
    }
}