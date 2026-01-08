namespace SmoothMovement
{
    public interface IMovementSettings
    {

        public float MaxSpeed { get; }
        public float Acceleration { get; }

        public SmoothStop SmoothStop { get; }
    }
}
