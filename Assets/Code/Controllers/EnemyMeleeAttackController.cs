using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class EnemyMeleeAttackController: IInitialization, IExecute
    {
        private readonly EnemyInitialization _enemyInitialization;
        private List<IEnemyMeleeModel> _enemies;
        
        public EnemyMeleeAttackController(EnemyInitialization enemyInitialization)
        {
            _enemyInitialization = enemyInitialization;
        }

        public void Initialization()
        {
            _enemies = _enemyInitialization.GetMeleeEnemies();
        }

        public void Execute(float deltaTime)
        {
            if (Time.frameCount % 2 != 0) 
                return;

            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                enemy.Cooldown -= deltaTime * 2;
                if (enemy.Cooldown >= 0)
                    continue;

                var position = enemy.View.AttackPoint.position;
                var forward = enemy.View.AttackPoint.forward;

                if (Physics.Raycast(position, forward, out var raycastHit, enemy.Data.AttackDistance))
                {
                    var unit = raycastHit.collider.gameObject.GetComponent<IUnitView>();
                    if (unit != null)
                    {
                        if (unit is IEnemyView || unit is IEnemyMeleeView)
                            continue;
                        
                        enemy.AudioSource.PlayOneShot(enemy.Data.AttackClip);
                        unit.AddDamage(enemy.GameObject, enemy.Data.AttackDamage);
                        enemy.Cooldown = enemy.Data.AttackRate;
                    }
                }
            }
        }
    }
}