using UnityEditor;
using UnityEngine;

namespace Widgets
{

    [CustomEditor(typeof(PointPath))]
    public class PointPathEditor : Editor
    {

        private readonly Color[] PointColors = {
            Color.cyan,
            Color.rebeccaPurple,
            Color.blue,
            Color.yellow, 
            Color.green,
            Color.orange
        };
        
        private readonly Color[] LineColors = {
            Color.white,
            Color.navyBlue
        };

        private PointPath _path;

        private void OnEnable()
        {
            _path = target as PointPath;
            //SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            //SceneView.duringSceneGui -= OnSceneGUI;
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_radius"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_thickness"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_points"), true);
            
            if (GUILayout.Button("Bake positions"))
            {
                _path.BakePositions(null);
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_bakedPositions"), true);

            serializedObject.ApplyModifiedProperties();
        }


        private void OnSceneGUI()
        {
            if (_path == null) return;

            for (int i = 0; i < _path.Points.Count; i++)
            {
                Vector3 worldPos = _path.GetPointWorldPosition(i);

                EditorGUI.BeginChangeCheck();
                
                Vector3 newPos = Handles.PositionHandle(worldPos, Quaternion.identity);

                Handles.color = GetColor(i, PointColors);
                Handles.SphereHandleCap(0, worldPos, Quaternion.identity, _path.Radius, EventType.Repaint);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_path, "Move Patrol Point");

                    var point = _path.Points[i];
                    point.localPosition = _path.transform.InverseTransformPoint(newPos);
                    _path.Points[i] = point;

                    EditorUtility.SetDirty(_path);
                }

                if (i > 0)
                {
                    Handles.color = GetColor(i, LineColors, false);
                    Handles.DrawLine(_path.GetPointWorldPosition(i - 1), worldPos, _path.Thickness);
                }
            }
        }

        private Color GetColor(int index, Color[] colors, bool changeEvery=true)
        {
            int length = colors.Length;

            if (changeEvery) return colors[index % length];
            
            return colors[index / length % length];
        }
    }
}