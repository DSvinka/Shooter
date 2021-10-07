using System;
using System.Collections.Generic;
using Code.Controllers.Initialization;
using Code.Interfaces;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Controllers
{
    internal sealed class EnemyMeleeController: IController, IInitialization, ICleanup
    {
        private readonly EnemyInitialization _initialization;
        
        private List<IEnemyMeleeModel> _enemiesMelee;

        public EnemyMeleeController(EnemyInitialization initialization)
        {
            _initialization = initialization;
        }
        
        public void Initialization()
        {
            _enemiesMelee = _initialization.GetMeleeEnemies();

            if (_enemiesMelee != null)
            {
                foreach (var enemy in _enemiesMelee)
                {
                    enemy.View.OnArmored += AddArmor;
                    enemy.View.OnHealing += AddHealth;
                    enemy.View.OnDamage += AddDamage;
                }   
            }
        }

        public void Cleanup()
        {
            foreach (var enemy in _enemiesMelee)
            {
                if (enemy != null)
                {
                    enemy.View.OnArmored -= AddArmor;
                    enemy.View.OnHealing -= AddHealth;
                    enemy.View.OnDamage -= AddDamage;   
                }
            }
        }

        private IEnemyMeleeModel GetEnemy(IUnitView unit)
        {
            var enemy = unit as IEnemyMeleeView;
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
            
            enemy.AudioSource.PlayOneShot(enemy.Data.GetDamageClip);

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
        
        private void Death(IEnemyMeleeModel enemyMelee)
        {
            // TODO: Добавить таймер с рандомом, чтобы не сразу спавнились черти.
            enemyMelee.Transform.position = enemyMelee.SpawnPoint.position;
            enemyMelee.AudioSource.Stop();
            enemyMelee.Reset();
            // _enemiesMelee.Remove(enemyMelee);
            // UnityEngine.Object.Destroy(enemyMelee.GameObject);
        }
    }
}