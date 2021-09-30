using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Data;
using Code.Interfaces.Units;
using UnityEngine;

namespace Code.Controllers
{
    internal sealed class EnemyMeleeController: IInitialization, IExecute
    {
        private readonly EnemyInitialization _enemyInitialization;
        private List<IEnemyMelee> _enemies;
        
        public EnemyMeleeController(EnemyInitialization enemyInitialization)
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
            
            foreach (var enemy in _enemies)
            {
                enemy.Cooldown -= deltaTime * 2;
                if (enemy.Cooldown >= 0) 
                    continue;
                
                var monoBehaviour = enemy as MonoBehaviour;
                if (monoBehaviour == null)
                    throw new Exception("MonoBehaviour не найден на объекте");
                
                var enemyComponent = enemy as IEnemy;
                if (enemyComponent == null)
                    throw new Exception("IEnemy не найден на объекте");
                
                var meleeData = enemyComponent.Data as IEnemyMeleeData;
                if (meleeData == null)
                    throw new Exception("IEnemyMeleeData не найден на объекте");
                  
                var position = enemy.AttackPoint.position;
                var forward = enemy.AttackPoint.forward;
                
                // TODO: Добавить звуки для зомби
                //_particleSystem.Play();
                //_audioSource.PlayOneShot(data.FireClip);

                if (Physics.Raycast(position, forward, out var raycastHit, meleeData.AttackDistance))
                {
                    var unit = raycastHit.collider.gameObject.GetComponent<IUnit>();
                    if (unit != null)
                    {
                        if (unit is IEnemy)
                            continue;
                        unit.AddDamage(monoBehaviour.gameObject, meleeData.AttackDamage);
                        enemy.Cooldown = meleeData.AttackRate;
                    }
                }
            }
        }
    }
}