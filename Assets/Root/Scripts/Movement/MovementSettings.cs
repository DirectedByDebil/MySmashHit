using UnityEngine;

namespace MySmashHit.Movement
{

    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Settings/MovementSettings")]
    public class MovementSettings : ScriptableObject, IMovementSettings
    {

        [field: SerializeField, Space, Range(0, 30)]
        public float MaxSpeed { get; private set; }


        [field: SerializeField, Space, Range(0, 100)]
        public float Acceleration { get; private set; }


        [field: SerializeField, Space]
        public SmoothStop SmoothStop { get; private set; }
    }
}
