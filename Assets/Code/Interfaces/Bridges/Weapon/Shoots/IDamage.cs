using UnityEngine;

namespace Code.Interfaces.Bridges.Weapon.Shoots
{
    public interface IDamage
    {
        void Damage(GameObject gameObject, Vector3 shootPoint);
    }
}