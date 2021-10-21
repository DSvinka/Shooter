using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Models;
using Code.Models;
using Code.SaveData;
using Code.UI;
using Code.UI.EscapeMenu;
using Code.UI.EscapeMenu.Windows;
using Code.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers.Player
{
    internal sealed class EscapeMenuController: IController, IInitialization, ICleanup, IExecute
    {
        private readonly SaveRepository _saveRepository;

        private readonly EnemyInitialization _enemyInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly UIInitialization _uiInitialization;
        
        private IBaseUI _currentWindow;
        private readonly Stack<EscapeMenuStateUI> _stateUi = new Stack<EscapeMenuStateUI>();

        private SettingsPanel _settingsPanel;

        private PlayerModel _player;
        private EscapeMenuView _escapeMenuView;

        private IUserKeyDownProxy _escapeInputProxy;
        private bool _escapeInput;

        public EscapeMenuController(SaveRepository saveRepository,
            PlayerInitialization playerInitialization, UIInitialization uiInitialization, EnemyInitialization enemyInitialization)
        {
            _saveRepository = saveRepository;

            _uiInitialization = uiInitialization;
            _enemyInitialization = enemyInitialization;
            _playerInitialization = playerInitialization;

            _escapeInputProxy = KeysInput.Escape;
        }
        
        public void Initialization()
        {
            _player = _playerInitialization.GetPlayer();
            _escapeMenuView = _uiInitialization.GetEscapeMenu();

            _settingsPanel = new SettingsPanel(_escapeMenuView.SettingsPanel);
            _settingsPanel.Cancel();

            _escapeMenuView.RestartGameButton.onClick.AddListener(OnRestartButtonClick);
            _escapeMenuView.SaveGameButton.onClick.AddListener(OnSaveButtonClick);
            _escapeMenuView.LoadGameButton.onClick.AddListener(OnLoadButtonClick);
            _escapeMenuView.SettingsGameButton.onClick.AddListener(OnSettingsButtonClick);
            _escapeMenuView.ExitGameButton.onClick.AddListener(OnExitButtonClick);

            _escapeInputProxy.KeyOnDown += OnEscapeInput;
        }
        
        private void OnEscapeInput(bool value) => _escapeInput = value;

        public void Execute(float deltaTime)
        {
            if (_escapeInput)
            {
                if (_stateUi.Count > 0)
                {
                    Close();
                    return;
                }

                var gameObject = _escapeMenuView.gameObject;
                var active = gameObject.activeSelf;

                if (active)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                _player.CanMove = active;
                if (_player.Weapon != null)
                    _player.Weapon.Blocking = !active;
                
                gameObject.SetActive(!active);
            }
        }

        private IBaseUI GetWindow(EscapeMenuStateUI escapeMenuStateUI)
        {
            var currentWindow = escapeMenuStateUI switch
            {
                EscapeMenuStateUI.Settings => _settingsPanel,
                _ => throw new ArgumentOutOfRangeException(nameof(escapeMenuStateUI),
                    $"{escapeMenuStateUI} не предусмотрен в EscapeMenuController")
            };
            return currentWindow;
        }
        
        private void Close()
        {
            if (_currentWindow == null || _stateUi.Count == 0)
                return;
            
            var state = _stateUi.Pop();
            var currentWindow = GetWindow(state);
            currentWindow.Cancel();
            _currentWindow = null;
        }
        private void Open(EscapeMenuStateUI escapeMenuStateUI, bool isSave = true)
        {
            if (_currentWindow != null)
            {
                Close();
            }

            _currentWindow = GetWindow(escapeMenuStateUI);
            _currentWindow.Execute();
            
            if (isSave)
            {
                _stateUi.Push(escapeMenuStateUI);
            }
        }

        private void OnRestartButtonClick()
        {
            // TODO: Заменить на нормальную перезагрузку через локации.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnSaveButtonClick()
        {
            _saveRepository.Save();
        }
        private void OnLoadButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnSettingsButtonClick()
        {
            Open(EscapeMenuStateUI.Settings);
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }

        public void Cleanup()
        {
            _escapeMenuView.RestartGameButton.onClick.RemoveListener(OnRestartButtonClick);
            _escapeMenuView.SaveGameButton.onClick.RemoveListener(OnSaveButtonClick);
            _escapeMenuView.LoadGameButton.onClick.RemoveListener(OnLoadButtonClick);
            _escapeMenuView.SettingsGameButton.onClick.RemoveListener(OnSettingsButtonClick);
            _escapeMenuView.ExitGameButton.onClick.RemoveListener(OnExitButtonClick);
            
            _escapeInputProxy.KeyOnDown -= OnEscapeInput;
        }
    }
}