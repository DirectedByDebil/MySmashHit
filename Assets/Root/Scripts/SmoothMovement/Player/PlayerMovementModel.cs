using MySmashHit.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


namespace SmoothMovement.Player
{
    internal class PlayerMovementModel
    {

        private readonly Rigidbody _rb;
        private readonly IPlayerMovementSettings _settings;

        private bool _isOnGround;
        private bool _canJump;
        private bool _isJumping;


        internal PlayerMovementModel(Rigidbody rb,
            IPlayerMovementSettings settings)
        {
            _rb = rb;
            _settings = settings;
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
                CoroutineManager.Instance.AddCoroutine("coyote time", CoyoteTime());
            }
        }

        #endregion


        internal void MakeMovement(Vector3 moveDirection)
        {

            moveDirection *= _settings.Acceleration;
            moveDirection.y = _rb.linearVelocity.y;

            if (_isJumping)
            {
                CoroutineManager.Instance.AddCoroutine("jump", SmoothJump());
                _isJumping = false;
                _canJump = false;
                _isOnGround = false;
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

                _isJumping = _canJump;
                //todo maybe interval in settings
                yield return new WaitForSeconds(0.2f);
            }
        }

        private IEnumerator SmoothJump()
        {

            SmoothJump smoothJump = _settings.SmoothJump;
            float duration = smoothJump.JumpTime;
            float height = smoothJump.Height;

            float upTime = duration / 2;
            Vector3 jumpPos = _rb.position;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {

                if (_isOnGround && !_isJumping)
                {
                    break;
                }

                float stage = elapsedTime < upTime ?
                    elapsedTime / upTime :
                    2f - elapsedTime / upTime;

                Vector3 newPos = _rb.position;
                newPos.y = jumpPos.y + height * smoothJump.JumpCurve.Evaluate(stage);

                _rb.position = newPos;

                elapsedTime += Time.fixedDeltaTime;
                yield return null;
            }
            
            yield return null;
        }

        private IEnumerator CoyoteTime()
        {
            yield return new WaitForSeconds(_settings.CoyoteTime);
            
            _canJump = _isOnGround;
        }

        #endregion
    }
}
