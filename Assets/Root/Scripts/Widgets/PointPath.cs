using UnityEngine;
using System;
using System.Collections.Generic;

namespace Widgets
{
    public class PointPath : MonoBehaviour
    {
        #region Properties

        public List<GUIPoint> Points => _points;

        public IReadOnlyList<Vector3> BakedPositions => _bakedPositions;

        public float Radius => _radius;
        public float Thickness => _thickness;

        #endregion

        [SerializeField, Space]
        private List<GUIPoint> _points = new ();
        
        [SerializeField, Space]
        private List<Vector3> _bakedPositions;

        [SerializeField, Range(0, 2f)]
        private float _radius;
        
        [SerializeField, Range(0, 20f)] 
        private float _thickness;


        public Vector3 GetPointWorldPosition(int index)
        {
            if (index < 0 || index >= _points.Count) return transform.position;
            return _points[index].GetWorldPosition(transform);
        }

        public void BakePositions(Transform owner)
        {
            Transform ownerTransform = owner != null ? owner: transform;

            _bakedPositions = new List<Vector3>(_points.Count);

            foreach (GUIPoint point in _points)
            {
                Vector3 pos = point.GetWorldPosition(ownerTransform);
                _bakedPositions.Add(pos);
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (_bakedPositions == null || _bakedPositions.Count == 0) return;

            for (int i = 0; i < _bakedPositions.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_bakedPositions[i], _radius);

                int nextIndex = (i + 1) % _bakedPositions.Count;
                Gizmos.color = Color.white;
                Gizmos.DrawLine(_bakedPositions[i], _bakedPositions[nextIndex]);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(_bakedPositions[i] + _radius * 2f * Vector3.up, i.ToString());
#endif
            }
        }

    }

    [Serializable]
    public class GUIPoint
    {
        public Vector3 localPosition;

        public Vector3 GetWorldPosition(Transform owner)
            => owner.TransformPoint(localPosition);
    }
}
