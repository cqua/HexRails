using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RailThroughway))]
public class RailThroughwayInspector : Editor {

	private void OnSceneGUI () {
		RailThroughway railway = target as RailThroughway;
		Transform handleTransform = railway.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		List<Vector3> points = new List<Vector3>();
		foreach(Vector3 p in railway.Points) {
			points.Add(handleTransform.TransformPoint(p));
		}

		if(points.Count > 1) {
			Handles.color = Color.white;

			for (int i = 0; i < points.Count; i++) {
				if (i + 1 < points.Count) {
					Handles.DrawLine(points[i], points[i + 1]);
				}

				EditorGUI.BeginChangeCheck();
				points[i] = Handles.DoPositionHandle(points[i], handleRotation);
				if (EditorGUI.EndChangeCheck()) {
					Undo.RecordObject(railway, "Move Point");
					EditorUtility.SetDirty(railway);
					railway.Points[i] = handleTransform.InverseTransformPoint(points[i]);
				}
			}
		}
	}
}