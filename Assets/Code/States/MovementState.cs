namespace Code.States
{
    internal abstract class MovementState
    {
        public abstract void Handle(MovementContext movementContext);
    }
}