using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Controllers
{
    internal sealed class EnemyController: IController, IInitialization, ICleanup
    {
        private readonly EnemyInitialization _initialization;

        private List<IEnemyModel> _enemies;

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
                    enemy.View.OnArmored += AddArmor;
                    enemy.View.OnHealing += AddHealth;
                    enemy.View.OnDamage += AddDamage;
                }   
            }
        }

        public void Cleanup()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null)
                {
                    enemy.View.OnArmored -= AddArmor;
                    enemy.View.OnHealing -= AddHealth;
                    enemy.View.OnDamage -= AddDamage;   
                }
            }
        }

        private IEnemyModel GetEnemy(IUnitView unit)
        {
            var enemy = unit as IEnemyView;
            if (enemy == null)
                throw new Exception("Unit не имеет интерфейса IEnemyView");
            return enemy.Model;
        }

        private void AddHealth(GameObject healer, IUnitView unit, float health)
        {
            var enemy = GetEnemy(unit);
            
            enemy.Health += health;
            if (enemy.Health > enemy.Data.MaxHealth)
                enemy.Health = enemy.Data.MaxHealth;
        }

        private void AddArmor(GameObject armorer, IUnitView unit, float armor)
        {
            var enemy = GetEnemy(unit);
            
            enemy.Armor += armor;
            if (enemy.Armor > enemy.Data.MaxArmor)
                enemy.Armor = enemy.Data.MaxArmor;
        }

        private void AddDamage(GameObject attacker, IUnitView unit, float damage)
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
        
        private void Death(IEnemyModel enemy)
        {
            _enemies.Remove(enemy);
            Object.Destroy(enemy.GameObject);
        }
    }
}