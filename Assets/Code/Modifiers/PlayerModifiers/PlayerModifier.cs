using Code.Models;

namespace Code.PlayerModifiers
{
    internal class PlayerModifier
    {
        protected PlayerModel _player;
        protected PlayerModifier Next;
    
        public PlayerModifier(PlayerModel player)
        {
            _player = player;
        }

        public void Add(PlayerModifier modifier)
        {
            if (Next != null)
            {
                Next.Add(modifier);
            }
            else
            {
                Next = modifier;
            }
        }

        public virtual void Handle() => Next?.Handle();

    }
}