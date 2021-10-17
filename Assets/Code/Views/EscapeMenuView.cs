using Code.UI;
using Code.UI.EscapeMenu;
using Code.UI.EscapeMenu.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Views
{
    internal sealed class EscapeMenuView: MonoBehaviour
    {
        [Header("Панель Кнопок")]
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private Button _saveGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _settingsGameButton;
        [SerializeField] private Button _exitGameButton;
        
        [Header("Окна")] 
        [SerializeField] private GameObject _settingsPanel;

        public Button RestartGameButton => _restartGameButton;
        public Button SaveGameButton => _saveGameButton;
        public Button LoadGameButton => _loadGameButton;
        public Button SettingsGameButton => _settingsGameButton;
        public Button ExitGameButton => _exitGameButton;

        public GameObject SettingsPanel => _settingsPanel;
    }
}