using UnityEngine;

namespace Code.Interfaces.Units
{
    public interface IEnemyMelee: IEnemy
    {
        Transform AttackPoint { get; }
        float Cooldown { get; set; }
    }
}