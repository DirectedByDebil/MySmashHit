using UnityEngine;

namespace World
{
    [CreateAssetMenu(fileName = "PlatformSettings", menuName = "Settings/PlatformSettings")]
    public class PlatformSettings : ScriptableObject, IPlatformSettings
    {

        [field: SerializeField, Header("Is waiting at point?")]
        public bool IsWaiting { get; private set; }


        [field: SerializeField, Space, Range(0f, 2f)]
        public float WaitTime {get; private set; }


        [field: SerializeField, Space, Range(0, 5f), Header("Min distance to point")]
        public float Accuracy {get; private set; }
    }
}
