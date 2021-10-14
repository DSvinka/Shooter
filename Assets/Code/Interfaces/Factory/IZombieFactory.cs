using Code.Interfaces.Bridges;
using Code.Interfaces.Data;
using Code.Interfaces.Models;
using Code.Managers;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IZombieFactory
    {
        IEnemyModel CreateZombie(Vector3 position, Vector3 rotation);
    }
}