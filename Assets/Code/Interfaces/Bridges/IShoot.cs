namespace Code.Interfaces.Bridges
{
    public interface IShoot
    {
        void MoveBullets(float deltaTime);
        void Shoot(float deltaTime);
    }
}