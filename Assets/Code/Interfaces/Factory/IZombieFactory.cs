using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Interfaces.Factory
{
    internal interface IZombieFactory
    {
        IEnemyModel CreateZombie(Vector3 position, Vector3 rotation);
    }
}