using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IUnitMeleeView: IUnitView
    {
        Transform AttackPoint { get; }
    }
}