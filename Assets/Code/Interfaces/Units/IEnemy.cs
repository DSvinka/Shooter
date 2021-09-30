using Code.Interfaces.Data;
using Code.Managers;

namespace Code.Interfaces.Units
{
    public interface IEnemy: IUnit
    {
        float Health { get; set; }
        float Armor { get; set; }
        IEnemyData Data { get; set; }
    }
}