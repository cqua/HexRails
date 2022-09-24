using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public abstract class RailSpline : MonoBehaviour {
	protected BezierSpline Spline;

	public List<Vector3> Points;

	public float railModelFrequency = .1f;

	public bool IsStation;

	private void Awake() {
		Spline = new BezierSpline();
		Spline.Initialize(Points);

		string railPrefabPath = IsStation ? "Prefabs/Rail-Red" 
			: "Prefabs/Rail";
		Object railPrefab = Resources.Load<Object>(railPrefabPath);

		if(railPrefab == null) {
			Debug.LogError("Failed to load Rail prefab.");
		} else {
			float t = 0;
			float distance = 0f;
			while(t < 1) {
				distance += SpeedModAtProgress(t);
				if(distance > railModelFrequency) {
					distance = 0;
					GameObject item = Instantiate(railPrefab) as GameObject;
					Vector3 position = Spline.GetPoint(t);
					item.transform.localPosition = position;
					item.transform.LookAt(GetDirection(t));
					item.transform.parent = this.transform;
				}
				t += .0001f;
			}
			for (int p = 0, f = 0; f < railModelFrequency; f++, p++) {
			}
		}

		InitializeExits();
	}

	protected abstract void InitializeExits();

	protected virtual RailSplineExit CreateExit(RailSpline to, bool fromIntersection, bool toIntersection) {
		if(to == null) {
			return new RailSplineExit();
		}
		return new RailSplineExit(this, to, fromIntersection, toIntersection);
	}

	public virtual Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = 1 - t;
		
		if (t >= 1) {
			return Spline.GetPoint(1);
		}

		return Spline.GetPoint(t);
	}

	public Vector3 GetDirection(float t, bool reverse = false) {
		if (!reverse) {
			return Spline.GetPoint(t + .001f);
			//return Spline.GetVelocity(t);
		} else {
			return Spline.GetPoint(t - .001f);
			//return Spline.GetVelocity(t) * -1;
		}
	}

	protected Vector3 GetPointAlongLine(Vector3 p0, Vector3 p1, float progress) {
		float x = p0.x * (1 - progress) + p1.x * progress;
		float y = p0.y * (1 - progress) + p1.y * progress;
		float z = p0.z * (1 - progress) + p1.z * progress;

		return transform.position + new Vector3(x, y, z);
	}

	public bool CarHasLeftRail(float progress) {
		return progress >= 1 || progress < 0;
	}

	public abstract RailSpline GetNextRail(RailCar car);

	//public float GetDistanceOfCurrentSpan(float t, bool reverse = false) {
	//	if (!reverse) {
	//		if (Length == 1 || t + 1 >= Length)
	//			return Vector3.Distance(Points[Length -1], Points[Length]);
	//		return Vector3.Distance(Points[Mathf.CeilToInt(t)], Points[Mathf.CeilToInt(t+1)]);
	//	} else {
	//		if (Length == 1 || t + 1 >= Length) {
	//			return Vector3.Distance(Points[0], Points[1]);
	//		}
	//		return Vector3.Distance(Points[Length - Mathf.CeilToInt(t) - 1], Points[Length - Mathf.CeilToInt(t)]);
	//	}
	//}

	public float SpeedModAtProgress(float t) {
		//if(t < 0)
		//	return 4f / Vector3.Distance(Spline.GetPoint(0), Spline.GetPoint(.01f));
		//if(t>1)
		//	return 4f / Vector3.Distance(Spline.GetPoint(.99f), Spline.GetPoint(1f));

		return 4f / Vector3.Distance(Spline.GetPoint(t - .001f), Spline.GetPoint(t + .001f));
	}
}
