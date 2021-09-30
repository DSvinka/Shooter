using Code.Interfaces.Data;
using UnityEngine;

namespace Code.Interfaces.Units
{
    public interface IEnemyMelee
    {
        Transform AttackPoint { get; }
        float Cooldown { get; set; }
    }
}