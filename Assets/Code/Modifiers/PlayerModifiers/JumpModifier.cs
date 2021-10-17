using Code.Models;

namespace Code.PlayerModifiers
{
    internal sealed class JumpModifier: PlayerModifier
    {
        private readonly float _jumpForce;

        public JumpModifier(PlayerModel player, float jumpForce) : base(player)
        {
            _jumpForce = jumpForce;
        }

        public override void Handle()
        {
            _player.JumpForce += _jumpForce;

            base.Handle();
        }
    }
}