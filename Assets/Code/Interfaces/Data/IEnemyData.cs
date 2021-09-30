using Code.Managers;

namespace Code.Interfaces.Data
{
    public interface IEnemyData: IUnitData
    {
        EnemyManager.EnemyType EnemyType { get; }
    }
}