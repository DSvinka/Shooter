using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Models;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Controllers
{
    internal sealed class EnemyMeleeMovementController: IInitialization, IExecute
    {
        private readonly EnemyInitialization _enemyInitialization;
        private readonly PlayerInitialization _playerInitialization;
        
        private List<IEnemyMeleeModel> _enemies;
        private PlayerModel _player;
        
        public EnemyMeleeMovementController(EnemyInitialization enemyInitialization, PlayerInitialization playerInitialization)
        {
            _enemyInitialization = enemyInitialization;
            _playerInitialization = playerInitialization;
        }

        public void Initialization()
        {
            _enemies = _enemyInitialization.GetMeleeEnemies();
            _player = _playerInitialization.GetPlayer();
        }

        public void Execute(float deltaTime)
        {
            if (Time.frameCount % 2 != 0) 
                return;

            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                
                enemy.NavMeshAgent.SetDestination(_player.Transform.position);
            }
        }
    }
}