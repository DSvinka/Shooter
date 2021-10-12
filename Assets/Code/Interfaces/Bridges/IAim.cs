using Code.Decorators;

namespace Code.Interfaces.Bridges
{
    internal interface IAim
    {
        IWeaponModification WeaponModification { get; }

        void OpenAim();
        void CloseAim();
    }
}