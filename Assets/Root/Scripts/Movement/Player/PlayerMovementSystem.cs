using MySmashHit.Helpers;
using UnityEngine;

namespace MySmashHit.Movement.Player
{
    /// <summary>
    /// Class for managing player movement depending on game conditions
    /// </summary>

    [RequireComponent(typeof(Rigidbody))]
    internal class PlayerMovementSystem : MonoBehaviour
    {

        [SerializeField]
        private PlayerMovementSettings _settings;

        private PlayerMovementController _controller;
        private PlayerMovementModel _model;

        private Rigidbody _rb;


        #region Unity Callbacks

        private void Awake() => Init();

        private void OnEnable() => SetMovement();

        private void OnDisable() => UnsetMovement();

        private void OnCollisionEnter(Collision collision) => _model.OnCollisionEnter(collision);

        private void OnCollisionExit(Collision collision) => _model.OnCollisionExit(collision);

        private void Update()
        {
            _controller.UpdateInput();

            _model.OnJumpChange(_controller.IsJumpPressed);
        }

        private void FixedUpdate() => _model.MakeMovement(_controller.Direction);

        private void OnGUI() { GUILogger.Instance.LogVelocity(_rb); }

        #endregion


        protected virtual void Init()
        {
            _rb = GetComponent<Rigidbody>();

            _controller = new PlayerMovementController();
            _model = new PlayerMovementModel(_rb, _settings, StartCoroutine);
        }


        protected virtual void SetMovement()
        {
            _controller.MoveCancelled += _model.OnMoveCancelled;    
        }

        protected virtual void UnsetMovement()
        {
            _controller.MoveCancelled -= _model.OnMoveCancelled;    
        }
    }
}
