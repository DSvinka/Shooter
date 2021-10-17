using Code.Views;

namespace Code.Interfaces.Factory
{
    internal interface IPlayerFactory: IFactory
    {
        PlayerView CreatePlayer();
    }
}