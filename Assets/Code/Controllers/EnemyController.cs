using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Controllers.Player;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Managers;
using Code.Models;
using Code.Services;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class EnemyController: IController, IInitialization, ICleanup, IExecute
    {
        private readonly EnemyInitialization _initialization;
        private readonly PlayerInitialization _playerInitialization;
        private readonly PlayerHudController _playerHudController;
        private readonly MessageBrokerService<string> _messageBrokerService;

        private Dictionary<int, IEnemyModel> _enemies;
        private PlayerModel _player;

        public EnemyController(EnemyInitialization initialization, PlayerInitialization playerInitialization, PlayerHudController playerHudController, MessageBrokerService<string> messageBrokerService)
        {
            _initialization = initialization;
            _playerInitialization = playerInitialization;
            _playerHudController = playerHudController;
            _messageBrokerService = messageBrokerService;
        }
        
        public void Initialization()
        {
            _enemies = _initialization.GetEnemies();
            _player = _playerInitialization.GetPlayer();

            if (_enemies.Count != 0)
            {
                foreach (var enemyDict in _enemies)
                {
                    var value = enemyDict.Value;
                    if (value.GameObject != null)
                    {
                        value.View.OnArmored += AddArmor;
                        value.View.OnHealing += AddHealth;
                        value.View.OnDamage += AddDamage;
                    }
                }   
            }
        }
        
        public void Execute(float deltaTime)
        {
            if (Time.frameCount % 2 != 0) 
                return;
            
            if (_enemies.Count == 0) 
                return;

            if (_player.GameObject == null)
                _player = _playerInitialization.GetPlayer();

            foreach (var enemyDict in _enemies)
            {
                var value = enemyDict.Value;
                if (value.GameObject == null)
                {
                    _enemies.Remove(enemyDict.Key);
                    continue;
                }
                    
                value.AttackBridge.Attack(deltaTime, value);
                value.MoveBridge.Move(deltaTime, value, _player.Transform.position);
            }
        }

        public void Cleanup()
        {
            if (_enemies.Count != 0)
            {
                foreach (var enemyDict in _enemies)
                {
                    var value = enemyDict.Value;
                    if (value.GameObject != null)
                    {
                        value.View.OnArmored -= AddArmor;
                        value.View.OnHealing -= AddHealth;
                        value.View.OnDamage -= AddDamage;
                    }
                }
            }
        }

        private void AddHealth(GameObject healer, int id, float health)
        {
            var enemy = _enemies[id];
            
            enemy.Health += health;
            if (enemy.Health > enemy.Data.MaxHealth)
                enemy.Health = enemy.Data.MaxHealth;
        }

        private void AddArmor(GameObject armorer, int id, float armor)
        {
            var enemy = _enemies[id];
            
            enemy.Armor += armor;
            if (enemy.Armor > enemy.Data.MaxArmor)
                enemy.Armor = enemy.Data.MaxArmor;
        }

        private void AddDamage(GameObject attacker, int id, float damage)
        {
            var enemy = _enemies[id];
            
            if (enemy.Armor > damage)
            {
                enemy.Armor -= damage;
                return;
            }

            if (enemy.Armor != 0)
            {
                damage -= enemy.Armor;
                enemy.Armor = 0;
            }

            enemy.Health -= damage;
            if (enemy.Health <= 0)
            {
                enemy.Health = 0;
                Death(id, enemy);
            }
        }
        
        private void Death(int id, IEnemyModel enemy)
        {
            var scoreOnDeath = enemy.Data.ScoreOnDeath;
            _playerHudController.AddScore(scoreOnDeath);
            _messageBrokerService.Publish(enemy.View, string.Format(MessagesManager.Enemy.DEATH, enemy.Data.Name, scoreOnDeath));
            
            // TODO: Добавить таймер с рандомом, чтобы не сразу спавнились черти.
            enemy.Transform.position = enemy.SpawnPointPosition;
            enemy.AudioSource.Stop();
            enemy.Reset();
            
            //_enemies.Remove(id);
            //Object.Destroy(enemy.GameObject);
        }
    }
}