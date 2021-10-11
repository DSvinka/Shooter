using Code.Interfaces.Input;

namespace Code.Input.Inputs
{
    internal sealed class KeysInput
    {
        public static IUserKeyDownProxy Escape { get; private set; }
        public static IUserKeyDownProxy Jump { get; private set; }
        public static IUserKeyDownProxy Reload { get; private set; }
        public static IUserKeyDownProxy Interact { get; private set; }
        public static IUserKeyDownProxy Drop { get; private set; }
        public static IUserKeyProxy Run { get; private set; }

        public KeysInput(IUserKeyDownProxy escape, IUserKeyDownProxy reload, IUserKeyDownProxy interact, IUserKeyDownProxy drop, IUserKeyDownProxy jump, IUserKeyProxy run)
        {
            Escape = escape;
            Jump = jump;
            Run = run;
            Reload = reload;
            Interact = interact;
            Drop = drop;
        }
    }
}