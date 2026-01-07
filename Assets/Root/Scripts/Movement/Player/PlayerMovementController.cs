using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace MySmashHit.Movement.Player
{
    internal class PlayerMovementController
    {

        internal event Action<InputAction.CallbackContext> MoveCancelled
        {
            add => _moveAction.canceled += value;
            remove => _moveAction.canceled -= value;
        }

        internal event Action<InputAction.CallbackContext> JumpStarted
        {
            add => _jumpAction.started += value;
            remove => _jumpAction.started -= value;
        }

        internal event Action<InputAction.CallbackContext> JumpCancelled
        {
            add => _jumpAction.canceled += value;
            remove => _jumpAction.canceled -= value;
        }

        internal Vector3 Direction { get; private set; }


        private readonly InputAction _moveAction;
        private readonly InputAction _jumpAction;


        internal PlayerMovementController ()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
        }


        internal void UpdateInput()
        {
            Vector3 moveDirection = _moveAction.ReadValue<Vector2>();
            (moveDirection.y, moveDirection.z) = (moveDirection.z, moveDirection.y);
            
            Direction = GetRelativeFromCamera(moveDirection);
        }


        private Vector3 GetRelativeFromCamera(Vector3 direction)
        {

            Transform cameraT = Camera.main.transform;

            direction = cameraT.forward * direction.z + cameraT.right * direction.x;
            direction.Normalize();

            return direction;
        }
    }
}
