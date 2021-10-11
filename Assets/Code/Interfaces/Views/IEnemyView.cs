using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IEnemyView: IUnitView
    {
        Transform AttackPoint { get; }
    }
}