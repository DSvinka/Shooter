using Code.Interfaces.Models;

namespace Code.Interfaces.Bridges
{
    public interface IAttack
    {
        void Attack(float deltaTime, IEnemyModel enemy);
    }
}