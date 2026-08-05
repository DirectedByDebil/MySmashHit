using UnityEngine;

namespace Core
{
    internal static class GameObjectUitls
    {
        public static bool IsPlayer(Collider other)
        {
            return other.CompareTag("Player");
        }

        public static bool IsPlayer(Collision other)
        {
            return other.gameObject.CompareTag("Player");
        }
    }
}