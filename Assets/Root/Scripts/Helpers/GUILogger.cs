using UnityEngine;
using System.Collections.Generic;

namespace MySmashHit.Helpers
{
    internal sealed class GUILogger : MonoBehaviour, IGUILogger
    {

        public static IGUILogger Instance { get => _instance; }
        private static IGUILogger _instance;

        [SerializeField, Space] private bool _isDebugging;
        [SerializeField, Space, Range(0, 50)] private float _lineHeight = 25;


        private Dictionary<string, string> _logs = new ();


        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }


        private void OnGUI()
        {

            if (!_isDebugging) return;


            Rect rect = new(30, 30, 250, 80);

            foreach (string log in _logs.Values)
            {
                GUI.Label(rect, log);
                rect.y += _lineHeight;
            }
        }


        public void LogVelocity(Rigidbody rb)
        {

            string playerVelocity = "player velocity";
            string linearVelocity = string.Format("Linear velocity: {0}", rb.linearVelocity);

            AddLog(playerVelocity, linearVelocity);


            string playerMagnitude = "player magnitude";

            Vector2 onGroundMovement = new(rb.linearVelocity.x, rb.linearVelocity.z);
            string magnitude = string.Format("Velocity magnitude: {0}", onGroundMovement.magnitude.ToString("F2"));

            AddLog(playerMagnitude, magnitude);
        }


        public void AddLog(string key, string value)
        {
            if (!_logs.TryAdd(key, value))
            {
                _logs[key] = value;
            }
        }
    }
}
