using System;
using Code.Interfaces.Input;
using Code.Managers;

namespace Code.Input
{
    internal sealed class InputModificationItemMenu: IUserKeyProxy
    {
        public event Action<bool> KeyOnChange = delegate(bool f) {  };

        public void GetKey()
        {
            KeyOnChange.Invoke(UnityEngine.Input.GetKey(KeysManager.MODIFICATION_ITEM_MENU));
        }
    }
}