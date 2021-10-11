using UnityEngine;

namespace Code.Interfaces.Views
{
    public interface IPlayerView: IUnitView
    {
        Transform AimPoint { get; }
        Transform HandPoint { get; }
    }
}