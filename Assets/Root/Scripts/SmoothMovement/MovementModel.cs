using UnityEngine;
using System.Collections;
using System;

namespace SmoothMovement
{
    public class MovementModel
    {

        protected readonly Rigidbody rb;
        protected readonly IMovementSettings settings;
       

        public MovementModel(Rigidbody rb, IMovementSettings settings)
        {
            this.rb = rb;
            this.settings = settings;
        }


        public virtual void MakeMovement(Vector3 moveDirection)
        {

            moveDirection *= settings.Acceleration;
            //moveDirection.y = rb.linearVelocity.y;

            moveDirection += rb.position;

            var (canMove, _) = CanMove(moveDirection);
            if (canMove)
            {
                //rb.AddForce(moveDirection, ForceMode.Force);
                rb.MovePosition(moveDirection);
            }
        }


        protected virtual ValueTuple<bool, bool> CanMove(Vector3 moveDirection)
        {
            Vector2 onGroundMovement = new(rb.linearVelocity.x, rb.linearVelocity.z);
            bool canMove = onGroundMovement.magnitude < settings.MaxSpeed;

            Vector2 input = new(moveDirection.x, moveDirection.z);
            bool isMoving = input.magnitude > 0f;


            return (canMove, isMoving);
        }

        protected virtual IEnumerator SmoothStop()
        {

            SmoothStop smoothStop = settings.SmoothStop;
            float maxTime = smoothStop.MaxTime;
            int maxSteps = smoothStop.Steps;

            float step = maxTime / maxSteps;
            var wait = new WaitForSeconds(step);

            float startStage = rb.linearVelocity.magnitude / settings.MaxSpeed;
            int remainedSteps = (int)(startStage * maxSteps);

            //GUILogger.Instance.AddLog("stop time", "stop time: " + (remainedSteps * step).ToString("F2"));
            do
            {
                //check if object stopped
                float stage = rb.linearVelocity.magnitude / settings.MaxSpeed;
                if (stage < 0.1f) { remainedSteps = 0; };

                float evaluate = smoothStop.StopCurve.Evaluate(remainedSteps * step);
                rb.linearVelocity *= evaluate;

                remainedSteps--;
                yield return wait;
            }
            while (remainedSteps > 0);
        }
    }
}
