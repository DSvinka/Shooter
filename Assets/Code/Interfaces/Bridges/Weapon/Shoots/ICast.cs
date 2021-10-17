using System.Collections.Generic;
using UnityEngine;

namespace Code.Interfaces.Bridges.Weapon.Shoots
{
    public interface ICast
    {
        public List<GameObject> Bullets { get; set; }
        
        void Cast(Vector3 origin, Vector3 direction);
    }
}