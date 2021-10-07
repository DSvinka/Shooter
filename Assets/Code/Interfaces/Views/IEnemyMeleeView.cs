using Code.Interfaces.Models;

namespace Code.Interfaces.Views
{
    public interface IEnemyMeleeView: IUnitMeleeView
    {
        IEnemyMeleeModel Model { get; set; }
    }
}