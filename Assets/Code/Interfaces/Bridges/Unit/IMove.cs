using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Interfaces.Bridges
{
    public interface IMove
    {
        void Move(float deltaTime, IEnemyModel enemy, Vector3 position);
    }
}