using UnityEngine;

namespace MySmashHit.Helpers
{
    public interface IGUILogger
    {
        public void AddLog(string key, string value);

        public void LogVelocity(Rigidbody rb);
    }
}
