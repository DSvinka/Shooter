using Code.Interfaces.Models;
using UnityEngine;

namespace Code.UI.EscapeMenu.Windows
{
    internal sealed class SettingsPanel: IBaseUI
    {
        private GameObject _gameObject;

        public SettingsPanel(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public void Execute()
        {
            _gameObject.SetActive(true);
        }

        public void Cancel()
        {
            _gameObject.SetActive(false);
        }
    }
}