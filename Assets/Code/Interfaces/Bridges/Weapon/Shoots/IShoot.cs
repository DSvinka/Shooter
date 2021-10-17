using Code.Decorators;

namespace Code.Interfaces.Bridges.Weapon.Shoots
{
    internal interface IShoot
    {
        public IWeaponModification WeaponModification { get; }

        void MoveBullets(float deltaTime);
        void Shoot(float deltaTime);
    }
}