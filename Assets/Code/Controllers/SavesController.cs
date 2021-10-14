using System.Collections.Generic;
using System.Linq;
using Code.Controllers.Initialization;
using Code.Input;
using Code.Input.Inputs;
using Code.Interfaces;
using Code.Interfaces.Input;
using Code.Interfaces.Models;
using Code.SaveData;
using Code.Utils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers
{
    internal sealed class SavesController : IController, IExecute, IInitialization, ICleanup
    {
        private readonly SaveRepository _saveRepository;
        private readonly PlayerInitialization _playerInitialization;
        private readonly EnemyInitialization _enemyInitialization;

        private Dictionary<int, IEnemyModel> _enemies;

        private IUserKeyDownProxy _reloadInputProxy;
        private bool _reloadInput;

        public SavesController(SaveRepository saveRepository, PlayerInitialization playerInitialization, EnemyInitialization enemyInitialization)
        {
            _saveRepository = saveRepository;
            _playerInitialization = playerInitialization;
            _enemyInitialization = enemyInitialization;
            
            _reloadInputProxy = KeysInput.SaveGame;
        }

        private void OnSaveGameInput(bool value) => _reloadInput = value;
        
        public void Cleanup()
        {
            _reloadInputProxy.KeyOnDown -= OnSaveGameInput;
        }

        public void Initialization()
        {
            _enemies = _enemyInitialization.GetEnemies();
            _reloadInputProxy.KeyOnDown += OnSaveGameInput;
        }
        
        // TODO: Временный костыль, потом нужно будет сделать через Escape меню.
        public void Execute(float deltaTime)
        {
            if (_reloadInput)
            {
                "Start Save".DebugLog();

                var player = _playerInitialization.GetPlayer();
                var enemyArray = new IEnemyModel[_enemies.Count];
                var enemyDictArray = _enemies.ToArray();
                for (var i = 0; i < enemyDictArray.Length; i++)
                {
                    enemyArray[i] = enemyDictArray[i].Value;
                } 
                
                _saveRepository.Save(player, enemyArray);
                "End Save, RESTART".DebugLog();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}