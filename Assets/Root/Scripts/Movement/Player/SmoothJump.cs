using UnityEngine;
using System;

namespace MySmashHit.Movement.Player
{

    [Serializable]
    public struct SmoothJump
    {

        [field: SerializeField, Space, Range(0f, 5f)]
        public float JumpTime { get; set; }


        [field: SerializeField, Space, Range(1, 20f)]
        public float Height { get; set; }


        [field: SerializeField, Space]
        public AnimationCurve JumpCurve { get; set; }
    }
}
