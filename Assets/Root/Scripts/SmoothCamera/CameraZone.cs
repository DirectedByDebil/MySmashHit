using MySmashHit.Helpers;
using UnityEngine;
using Cinemachine;

namespace SmoothCamera
{
    [RequireComponent (typeof(Collider))]
    internal class CameraZone : MonoBehaviour
    {
        [SerializeField, Space]
        private CinemachineVirtualCameraBase _camera;
        
        [SerializeField, Header("Should camera switch back when exit zone?")]
        private bool _isLocal = true;

        [SerializeField, Space, Range(1, 1000)]
        private int _priority = 100;

        private int _normalPriority;


        private void OnTriggerEnter(Collider other)
        {

            if (GameObjectUitls.IsPlayer(other))
            {
                _normalPriority = _camera.Priority;
                _camera.Priority = _priority;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (_isLocal &&  GameObjectUitls.IsPlayer(other))
            {
                _camera.Priority = _normalPriority;
            }
        }
    }
}
