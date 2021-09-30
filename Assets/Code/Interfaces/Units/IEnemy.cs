using Code.Interfaces.Data;

namespace Code.Interfaces.Units
{
    public interface IEnemy: IUnit
    {
        float Health { get; set; }
        float Armor { get; set; }
        IEnemyData Data { get; set; }
    }
}