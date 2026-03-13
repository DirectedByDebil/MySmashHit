using MySmashHit.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;


namespace SmoothMovement.Player
{
    internal class PlayerMovementModel
    {

        private readonly Rigidbody _rb;
        private readonly IPlayerMovementSettings _settings;

        private readonly WaitForSeconds _bufferInput;
        private readonly WaitForSeconds _coyoteTime;


        private InputState _jumpState;

        private bool _isOnGround;
        private bool _canJump;
        private bool _isJumping;


        internal PlayerMovementModel(Rigidbody rb,
            IPlayerMovementSettings settings)
        {
            _rb = rb;
            _settings = settings;

            _bufferInput = new WaitForSeconds(0.2f);
            _coyoteTime = new WaitForSeconds(_settings.CoyoteTime);
        }


        #region On Input Changed

        internal virtual void OnMoveCancelled(InputAction.CallbackContext context)
        {
            if (_isOnGround)
            {
                CoroutineManager.Instance.AddCoroutine("smooth stop", SmoothStop());
            }
        }


        internal virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            CoroutineManager.Instance.AddCoroutine("try jump", TryJump());
        }

        internal virtual void OnJumpCancelled(InputAction.CallbackContext context)
        {
            CoroutineManager.Instance.RemoveCoroutine("try jump");
        }


        #endregion


        #region On Collision Callbacks

        internal void OnCollisionEnter(Collision collision)
        {
            //stop jump if collapsed
            _isJumping = false;

            if (collision.gameObject.CompareTag("Ground"))
            {
                _isOnGround = true;
                _canJump = true;
            }
        }

        internal void OnCollisionExit(Collision collision)
        {

            if (collision.gameObject.CompareTag("Ground"))
            {
                _isOnGround = false;
                CoroutineManager.Instance.RemoveCoroutine("coyote time");
                CoroutineManager.Instance.AddCoroutine("coyote time", CoyoteTime());
            }
        }

        #endregion


        internal void MakeMovement(Vector3 moveDirection)
        {

            moveDirection *= _settings.Acceleration;
            moveDirection.y = _rb.linearVelocity.y;

            GUILogger.Instance.AddLog("move dir", moveDirection.normalized.ToString());
            if (_jumpState == InputState.Pending)
            {
                CoroutineManager.Instance.AddCoroutine("jump", SmoothJump());

                _jumpState = InputState.Executed;
                _canJump = false;
                _isOnGround = false;
            }

            var (canMove, _) = CanMove(moveDirection);
            if (canMove)
            {
                _rb.AddForce(moveDirection, ForceMode.Force);
            }
        }

        
        private ValueTuple<bool, bool> CanMove(Vector3 moveDirection)
        {
            Vector2 onGroundMovement = new(_rb.linearVelocity.x, _rb.linearVelocity.z);
            bool canMove = onGroundMovement.magnitude < _settings.MaxSpeed;
         
            Vector2 input = new(moveDirection.x, moveDirection.z);
            bool isMoving = input.magnitude > 0f;


            return (canMove, isMoving);
        }


        #region Coroutines

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

        private IEnumerator TryJump()
        {

            while (true)
            {
                if (_canJump)
                {
                    _jumpState = InputState.Pending;
                }

                yield return _bufferInput;
            }
        }

        private IEnumerator SmoothJump()
        {

            SmoothJump smoothJump = _settings.SmoothJump;
            float duration = smoothJump.JumpTime;
            float height = smoothJump.Height;

            float upTime = duration / 2;
            Vector3 jumpPos = _rb.position;

            var wait = new WaitForFixedUpdate();

            _isJumping = true;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                // Early jump termination condition
                // Jump is interrupted if:
                // 1. Player touches the ground AND the jump button is no longer active (released or buffer expired)
                // 2. OR jump was forcefully stopped externally (ceiling collision, etc.)
                if (_isOnGround && _jumpState != InputState.Pending || !_isJumping)
                {
                    // stop jumping
                    _isJumping = false;
                    break;
                }
                
                float stage = elapsedTime < upTime ?
                    elapsedTime / upTime :
                    2f - elapsedTime / upTime;

                Vector3 newPos = _rb.position;
                newPos.y = jumpPos.y + height * smoothJump.JumpCurve.Evaluate(stage);

                _rb.MovePosition(newPos);

                elapsedTime += Time.fixedDeltaTime;
                yield return wait;
            }

            _isJumping = false;
            _jumpState = InputState.None;
            yield return wait;
        }

        private IEnumerator CoyoteTime()
        {
            yield return _coyoteTime;

            _canJump = _isOnGround;
        }

        #endregion
    }
}
