using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Units;
using Code.Views;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Controllers
{
    internal sealed class EnemyMeleeMovementController: IInitialization, IExecute
    {
        private readonly EnemyInitialization _enemyInitialization;
        private readonly PlayerInitialization _playerInitialization;
        private PlayerView _playerView;
        private List<IEnemyMelee> _enemies;
        
        public EnemyMeleeMovementController(EnemyInitialization enemyInitialization, PlayerInitialization playerInitialization)
        {
            _enemyInitialization = enemyInitialization;
            _playerInitialization = playerInitialization;
        }

        public void Initialization()
        {
            _enemies = _enemyInitialization.GetMeleeEnemies();
            _playerView = _playerInitialization.GetPlayer();
        }

        public void Execute(float deltaTime)
        {
            if (Time.frameCount % 2 != 0) 
                return;

            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                var monoBehaviour = enemy as MonoBehaviour;
                if (monoBehaviour == null)
                    throw new Exception("MonoBehaviour не найден на Enemy объекте");

                var navMeshAgent = monoBehaviour.GetComponent<NavMeshAgent>();
                navMeshAgent.SetDestination(_playerView.transform.position);
            }
        }
    }
}