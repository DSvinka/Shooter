using Code.Interfaces.Data;
using Code.Interfaces.Views;

namespace Code.Interfaces.Models
{
    public interface IEnemyModel: IUnitModel
    {
        IEnemyView View { get; }
        IEnemyData Data { get; }
    }
}