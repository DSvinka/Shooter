using Code.Interfaces.Bridges;
using Code.Interfaces.Data;
using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IEnemyFactory
    {
        IEnemyModel CreateEnemy(IEnemyData data, GameObject prefab, IMove moveBridge, IAttack attackBridge, Transform spawnPoint);
    }
}