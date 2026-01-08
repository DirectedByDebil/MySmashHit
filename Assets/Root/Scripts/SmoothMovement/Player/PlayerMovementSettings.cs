using UnityEngine;

namespace SmoothMovement.Player
{
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Settings/PlayerMovementSettings")]
    internal sealed class PlayerMovementSettings : MovementSettings, IPlayerMovementSettings
    {

        [field: SerializeField, Space, Range(0f, 1f)]
        public float CoyoteTime { get; private set; }


        [field: SerializeField, Space]
        public SmoothJump SmoothJump { get; private set; }
    }
}