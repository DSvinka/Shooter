using Code.Models;

namespace Code.PlayerModifiers
{
    internal sealed class ArmorModifier: PlayerModifier
    {
        private readonly int _armor;

        public ArmorModifier(PlayerModel player, int armor) : base(player)
        {
            _armor = armor;
        }

        public override void Handle()
        {
            _player.Armor += _armor;

            base.Handle();
        }
    }
}