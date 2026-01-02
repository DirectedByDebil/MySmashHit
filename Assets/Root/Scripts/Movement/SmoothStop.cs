using UnityEngine;
using System;

namespace MySmashHit.Movement
{

    [Serializable]
    public struct SmoothStop
    {

        [field: SerializeField, Space, Range(0f, 2f)]
        public float MaxTime { get; set; }
        

        [field: SerializeField, Space, Range(1, 10)]
        public int Steps { get; set; }


        [field: SerializeField, Space]
        public AnimationCurve StopCurve { get; set; }
    }
}
