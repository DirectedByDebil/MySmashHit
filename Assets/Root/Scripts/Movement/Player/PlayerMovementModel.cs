using MySmashHit.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

namespace MySmashHit.Movement.Player
{
    internal class PlayerMovementModel
    {

        private readonly Rigidbody _rb;
        private readonly IPlayerMovementSettings _settings;
        private readonly Func<IEnumerator, Coroutine> _startCoroutine;

        private bool _isOnGround;
        private bool _isJumping;


        internal PlayerMovementModel(Rigidbody rb,
            IPlayerMovementSettings settings,
            Func<IEnumerator, Coroutine> startCoroutine)
        {
            _rb = rb;
            _settings = settings;
            _startCoroutine = startCoroutine;
        }


        #region On Input Changed

        internal virtual void OnMoveCancelled(InputAction.CallbackContext context)
        {
            if (_isOnGround)
            {
                _startCoroutine(SmoothStop());
            }
        }

        //todo check this
        // if do _isJumping false maybe better use callbacks
        internal virtual void OnJumpChange(bool isJumped)
        {
            _isJumping = _isOnGround && isJumped;
        }

        #endregion


        #region On Collision Callbacks

        internal void OnCollisionEnter(Collision collision)
        {

            if (collision.gameObject.CompareTag("Ground"))
            {
                _isOnGround = true;
            }
        }

        internal void OnCollisionExit(Collision collision)
        {

            if (collision.gameObject.CompareTag("Ground"))
            {
                _isOnGround = false;
            }
        }

        #endregion


        internal void MakeMovement(Vector3 moveDirection)
        {

            moveDirection *= _settings.Acceleration;
            moveDirection.y = _rb.linearVelocity.y;

            if (_isJumping)
            {

                _rb.AddForce(Vector3.up * _settings.JumpHeight, ForceMode.Impulse);

                _isJumping = false;
            }


            if (CanMove())
            {
                _rb.AddForce(moveDirection, ForceMode.Force);
            }
        }


        private bool CanMove()
        {

            Vector2 onGroundMovement = new(_rb.linearVelocity.x, _rb.linearVelocity.z);

            return onGroundMovement.magnitude < _settings.MaxSpeed;
        }

        
        private IEnumerator SmoothStop()
        {

            SmoothStop smoothStop = _settings.SmoothStop;
            float maxTime = smoothStop.MaxTime;
            int maxSteps = smoothStop.Steps;

            float step = maxTime / maxSteps;
            var wait = new WaitForSeconds(step);

            float startStage = _rb.linearVelocity.magnitude / _settings.MaxSpeed;
            int remainedSteps = (int)(startStage * maxSteps);

            GUILogger.Instance.AddLog("stop time", "stop time: " + (remainedSteps * step).ToString("F2"));
            do
            {
                //check if object stopped
                float stage = _rb.linearVelocity.magnitude / _settings.MaxSpeed;
                if (stage < 0.1f) { remainedSteps = 0; };

                float evaluate = smoothStop.StopCurve.Evaluate(remainedSteps * step);
                _rb.linearVelocity *= evaluate;

                remainedSteps--;
                yield return wait;
            }
            while (remainedSteps > 0);
        }
    }
}
