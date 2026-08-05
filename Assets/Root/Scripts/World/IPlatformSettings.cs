namespace World
{
    public interface IPlatformSettings
    {
        public bool IsWaiting {get;}

        public float WaitTime {get;}

        public float Accuracy { get; }
    }
}
