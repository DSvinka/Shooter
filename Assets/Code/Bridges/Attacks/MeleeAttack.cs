using Code.Interfaces.Bridges;
using Code.Interfaces.Models;
using Code.Interfaces.Views;
using UnityEngine;

namespace Code.Bridges.Attacks
{
    internal sealed class MeleeAttack: IAttack
    {
        public void Attack(float deltaTime, IEnemyModel enemy)
        {
            enemy.AttackCooldown -= deltaTime * 2;
            if (enemy.AttackCooldown >= 0)
                return;

            var position = enemy.View.AttackPoint.position;
            var forward = enemy.View.AttackPoint.forward;

            if (Physics.Raycast(position, forward, out var raycastHit, enemy.Data.AttackDistance))
            {
                var unit = raycastHit.collider.gameObject.GetComponent<IUnitView>();
                if (unit != null)
                {
                    if (unit is IEnemyView)
                        return;
                    
                    enemy.AudioSource.PlayOneShot(enemy.Data.AttackClip);
                    unit.AddDamage(enemy.GameObject, enemy.Data.AttackDamage);
                    enemy.AttackCooldown = enemy.Data.AttackRate;
                }
            }
        }
    }
}