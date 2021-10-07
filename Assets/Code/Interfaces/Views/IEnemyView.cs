using Code.Interfaces.Models;

namespace Code.Interfaces.Views
{
    public interface IEnemyView: IUnitView
    {
        IEnemyModel Model { get; set; }
    }
}