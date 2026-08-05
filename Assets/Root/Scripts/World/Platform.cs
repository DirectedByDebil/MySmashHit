using Widgets;
using SmoothMovement;
using UnityEngine;
using System.Collections;


namespace World
{
    [RequireComponent(typeof(PointPath), typeof(Rigidbody))]
    public class Platform: MonoBehaviour
    {

        private enum PlatformState
        {
            Moving,
            Staying
        }

        [SerializeField]
        private PlatformSettings _platformSettings;

        [SerializeField, Space]
        private MovementSettings _movementSettings;

        private Rigidbody _rb;
        private PointPath _path;

        private MovementModel _movementModel;

        private int _currentIndex;        
        private PlatformState _state;
        private Vector3 _moveDirection;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();

            _path = GetComponent<PointPath>();
            _path.BakePositions(transform);

            _movementModel = new MovementModel(_rb, _movementSettings);

            _currentIndex = 0;

            _state = PlatformState.Moving;
        }


        //message if settings null
        private void OnValidate() { }


        private void FixedUpdate()
        {

            if (_state == PlatformState.Moving)
            {
                CountMoveDirection();
                _movementModel.MakeMovement(_moveDirection.normalized);
            }

            if (_state != PlatformState.Staying && IsReached())
            {
                _state = PlatformState.Staying;

                _currentIndex++;
                if (_currentIndex >= _path.BakedPositions.Count)
                {
                    _currentIndex = 0;
                }

                if (_platformSettings.IsWaiting)
                {
                    StartCoroutine(Wait());
                }
                else
                {
                    _state = PlatformState.Moving;
                }
            }
        }


        private bool IsReached()
        {
            return _moveDirection.magnitude < _platformSettings.Accuracy;
        }

        private Vector3 CountMoveDirection()
        {
            _moveDirection = _path.BakedPositions[_currentIndex] - _rb.position;
            return _moveDirection;
        }


        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(_platformSettings.WaitTime);

            _state = PlatformState.Moving;
            CountMoveDirection();
        }

    }
}
