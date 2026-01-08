namespace SmoothMovement.Player
{
    public interface IPlayerMovementSettings : IMovementSettings
    {

        public float CoyoteTime { get; }

        public SmoothJump SmoothJump { get; }
    }
}
