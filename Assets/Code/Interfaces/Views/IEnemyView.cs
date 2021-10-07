using Code.Interfaces.Models;
using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IEnemyView: IUnitView
    {
        Transform AttackPoint { get; }
    }
}