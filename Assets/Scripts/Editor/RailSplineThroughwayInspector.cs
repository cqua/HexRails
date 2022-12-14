using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RailThroughwayController))]
public class RailSplineThroughwayInspector : Editor {

	private void OnSceneGUI () {
		RailThroughwayController railway = target as RailThroughwayController;
		Transform handleTransform = railway.transform;
		Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

		List<Vector3> points = new List<Vector3>();
		foreach(Vector3 p in railway.Points) {
			points.Add(handleTransform.TransformPoint(p));
		}

		if(points.Count >= 4) {
			BezierSpline spline = new BezierSpline(railway.Points);
			Handles.color = Color.white;

			for (float i = 0; i < 1; i+=.01f) {
				Handles.DrawLine(spline.GetPoint(i) + handleTransform.position, spline.GetPoint(i + .01f) + handleTransform.position);
			}

			for (int i = 0; i < points.Count; i++) {

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