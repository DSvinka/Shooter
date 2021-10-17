using Code.Managers;
using Code.Models;

namespace Code.PlayerModifiers
{
    internal sealed class SpeedModifier: PlayerModifier
    {
        private readonly float _walkingSpeed;
        private readonly float _runningSpeed;

        public SpeedModifier(PlayerModel player, float walkingSpeed, float runningSpeed) : base(player)
        {
            _walkingSpeed = walkingSpeed;
            _runningSpeed = runningSpeed;
        }

        public override void Handle()
        { 
            _player.WalkingSpeed += _walkingSpeed;
            _player.RunningSpeed += _runningSpeed;
            base.Handle();
        }
    }
}