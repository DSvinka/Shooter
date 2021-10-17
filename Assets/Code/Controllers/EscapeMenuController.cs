using System.Collections.Generic;
using System.Linq;
using Code.Controllers.Initialization;
using Code.Data.DataStores;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Models;
using Code.Models;
using Code.SaveData;
using Code.Views;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers
{
    internal sealed class EscapeMenuController: IController, IInitialization, ICleanup
    {
        private readonly SaveRepository _saveRepository;
        private readonly EnemyInitialization _enemyInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly UIInitialization _uiInitialization;

        private PlayerModel _player;
        private EscapeMenuView _escapeMenuView;
        private Dictionary<int, IEnemyModel> _enemies;
        
        private IUserKeyDownProxy _escapeInputProxy;
        private bool _escapeInput;

        public EscapeMenuController(SaveRepository saveRepository, PlayerInitialization playerInitialization, UIInitialization uiInitialization, EnemyInitialization enemyInitialization)
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
            _enemies = _enemyInitialization.GetEnemies();
            _escapeMenuView = _uiInitialization.GetEscapeMenu();

            _escapeMenuView.RestartGameButton.onClick.AddListener(OnRestartButtonClick);
            _escapeMenuView.SaveGameButton.onClick.AddListener(OnSaveButtonClick);
            _escapeMenuView.LoadGameButton.onClick.AddListener(OnLoadButtonClick);
            _escapeMenuView.SettingsGameButton.onClick.AddListener(OnSettingsButtonClick);
            _escapeMenuView.ExitGameButton.onClick.AddListener(OnExitButtonClick);

            _escapeInputProxy.KeyOnDown += OnEscapeInput;
        }

        private void OnEscapeInput(bool value)
        {
            if (value)
            {
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

        private void OnRestartButtonClick()
        {
            // TODO: Заменить на нормальную перезагрузку через локации.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnSaveButtonClick()
        {
            var enemyArray = new IEnemyModel[_enemies.Count];
            var enemyDictArray = _enemies.ToArray();
            for (var i = 0; i < enemyDictArray.Length; i++)
            {
                enemyArray[i] = enemyDictArray[i].Value;
            } 
                
            _saveRepository.Save(_player, enemyArray);
        }
        
        private void OnLoadButtonClick()
        {
            // TODO: Сделать выход из игры
        }
        
        private void OnSettingsButtonClick()
        {
            // TODO: Заменить на нормальную перезагрузку через локации.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnExitButtonClick()
        {
            // TODO: Сделать выход из игры
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