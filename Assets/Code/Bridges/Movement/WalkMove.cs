using Code.Interfaces.Bridges;
using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Bridges.Movement
{
    internal sealed class WalkMove: IMove
    {
        public void Move(float deltaTime, IEnemyModel enemy, Vector3 position)
        {
            enemy.NavMeshAgent.SetDestination(position);
        }
    }
}