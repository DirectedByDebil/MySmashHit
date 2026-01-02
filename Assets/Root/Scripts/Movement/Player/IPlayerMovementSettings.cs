namespace MySmashHit.Movement.Player
{
    public interface IPlayerMovementSettings : IMovementSettings
    {
        public float JumpHeight { get; }
    }
}
