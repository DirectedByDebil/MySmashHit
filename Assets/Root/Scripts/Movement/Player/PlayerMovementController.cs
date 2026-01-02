using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace MySmashHit.Movement.Player
{
    internal class PlayerMovementController
    {

        internal event Action<InputAction.CallbackContext> MoveCancelled
        {
            add { _moveAction.canceled += value; }
            remove { _moveAction.canceled -= value; }
        }

        internal Vector3 Direction { get; private set; }
        internal bool IsJumpPressed { get => _jumpAction.IsPressed(); }


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

            Direction = moveDirection;
        }
    }
}
