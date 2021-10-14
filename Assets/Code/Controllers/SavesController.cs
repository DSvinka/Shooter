using System.Collections.Generic;
using System.Linq;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.SaveData;
using Code.Utils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Controllers
{
    internal sealed class SavesController : IController, IExecute, IInitialization
    {
        private readonly SaveRepository _saveRepository;
        private readonly PlayerInitialization _playerInitialization;
        private readonly EnemyInitialization _enemyInitialization;

        private Dictionary<int, IEnemyModel> _enemies;
        private bool _saved;

        public SavesController(SaveRepository saveRepository, PlayerInitialization playerInitialization, EnemyInitialization enemyInitialization)
        {
            _saveRepository = saveRepository;
            _playerInitialization = playerInitialization;
            _enemyInitialization = enemyInitialization;
        }

        public void Initialization()
        {
            _enemies = _enemyInitialization.GetEnemies();
        }
        
        public void Execute(float deltaTime)
        {
            // TODO: Временный костыль
            if (!_saved && UnityEngine.Input.GetKey(KeyCode.Delete))
            {
                "Start Save".DebugLog();
                _saved = true;
                
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