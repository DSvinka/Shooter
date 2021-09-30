using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    internal sealed class EnemyController: IController, IInitialization, ICleanup
    {
        private readonly EnemyInitialization _initialization;

        private List<IEnemy> _enemies;

        public EnemyController(EnemyInitialization initialization)
        {
            _initialization = initialization;
        }
        
        public void Initialization()
        {
            _enemies = _initialization.GetEnemies();

            if (_enemies != null)
            {
                foreach (var enemy in _enemies)
                {
                    enemy.OnArmored += AddArmor;
                    enemy.OnHealing += AddHealth;
                    enemy.OnDamage += AddDamage;
                }   
            }
        }

        public void Cleanup()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null)
                {
                    enemy.OnArmored -= AddArmor;
                    enemy.OnHealing -= AddHealth;
                    enemy.OnDamage -= AddDamage;   
                }
            }
        }

        private IEnemy GetEnemy(IUnit unit)
        {
            var enemy = unit as IEnemy;
            if (enemy == null)
                throw new Exception("Unit не имеет интерфейса IEnemy");
            return enemy;
        }

        private void AddHealth(GameObject healer, IUnit unit, float health)
        {
            var enemy = GetEnemy(unit);
            
            enemy.Health += health;
            if (enemy.Health > enemy.Data.MaxHealth)
                enemy.Health = enemy.Data.MaxHealth;
        }

        private void AddArmor(GameObject armorer, IUnit unit, float armor)
        {
            var enemy = GetEnemy(unit);
            
            enemy.Armor += armor;
            if (enemy.Armor > enemy.Data.MaxArmor)
                enemy.Armor = enemy.Data.MaxArmor;
        }

        private void AddDamage(GameObject attacker, IUnit unit, float damage)
        {
            var enemy = GetEnemy(unit);
            
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
                Death(enemy);
            }
        }
        
        private void Death(IEnemy enemy)
        {
            var monoBehaviour = enemy as MonoBehaviour;
            if (monoBehaviour == null)
                throw new Exception("Enemy не имеет класса MonoBehaviour");
            
            _initialization.GetEnemies().Remove(enemy);
            if (enemy is IEnemyMelee enemyMelee)
                _initialization.GetMeleeEnemies().Remove(enemyMelee);
            
            Object.Destroy(monoBehaviour.gameObject);
        }
    }
}