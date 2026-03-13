using UnityEngine;

namespace MySmashHit.Helpers
{
    internal static class GameObjectUitls
    {
        public static bool IsPlayer(Collider other)
        {
            return other.CompareTag("Player");
        }
    }
}