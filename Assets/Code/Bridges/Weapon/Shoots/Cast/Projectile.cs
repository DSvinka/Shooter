using System.Collections.Generic;
using Code.Interfaces.Bridges.Weapon.Shoots;
using UnityEngine;

namespace Code.Bridges.Weapon.Shoots.Cast
{
    internal sealed class Projectile: ICast
    {
        // TODO: Реализовать этот класс
        public List<GameObject> Bullets { get; set; }

        public void Cast(Vector3 origin, Vector3 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}