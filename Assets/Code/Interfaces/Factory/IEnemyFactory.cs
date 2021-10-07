using Code.Interfaces.Data;
using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IEnemyFactory
    {
        IEnemyModel CreateEnemy(IEnemyData data, GameObject prefab, Transform spawnPoint);
        
        IEnemyMeleeModel CreateMeleeEnemy(IEnemyMeleeData data, GameObject prefab, Transform spawnPoint);
    }
}