using UnityEngine;

namespace MySmashHit.Movement.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Settings/PlayerMovementSettings")]
    internal sealed class PlayerMovementSettings : MovementSettings, IPlayerMovementSettings
    {

        [field: SerializeField, Space, Range(0, 80)]
        public float JumpHeight { get; private set; }
    }
}