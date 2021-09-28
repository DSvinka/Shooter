using UnityEngine;

namespace Code.Interfaces.Factory
{
    public interface IPlayerFactory: IFactory
    {
        Transform CreatePlayer();
        Transform CreatePlayerHud();
    }
}