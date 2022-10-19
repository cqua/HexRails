using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public abstract class RailController : MonoBehaviour {
	protected BezierSpline Spline;

	public bool IsStation;
	public List<Vector3> Points;

	const float RAIL_MODEL_FREQ = .002f;

	private void Awake() {
		Spline = new BezierSpline(Points);

		DrawRail();

		InitializeExits();
	}

	protected abstract void InitializeExits();

	private void DrawRail() {
		string railPrefabPath =
			IsStation ? "Prefabs/Rail-Red" : "Prefabs/Rail";
		Object railPrefab = Resources.Load<Object>(railPrefabPath);

		if (railPrefab == null) {
			Debug.LogError("Failed to load Rail prefab.");
		} else {
			float progress = 0;
			float distanceToNextRail = 0f;
			while (progress + distanceToNextRail < 1) {
				distanceToNextRail += .0001f * SpeedModAtProgress(progress);
				if (distanceToNextRail > RAIL_MODEL_FREQ) {
					progress += distanceToNextRail;
					distanceToNextRail = 0;

					GameObject item = Instantiate(railPrefab) as GameObject;
					Vector3 position = GetPoint(progress);
					item.transform.localPosition = position;
					item.transform.LookAt(GetDirection(progress));
					item.transform.parent = this.transform;
				}
			}
		}
	}

	protected virtual RailExit CreateExit(RailController to, bool fromIntersection, bool toIntersection) {
		if(to == null) {
			return new RailExit();
		}
		return new RailExit(this, to, fromIntersection, toIntersection);
	}

	public virtual Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = 1 - t;
		
		if (t >= 1) {
			return Spline.GetPoint(1) + transform.position;
		}

		return Spline.GetPoint(t) + transform.position;
	}

	public Vector3 GetDirection(float t, bool reverse = false) {
		if (!reverse) {
			return Spline.GetPoint(t + .001f) + transform.position;
		} else {
			return Spline.GetPoint(1 - t - .001f) + transform.position;
		}
	}

	public bool CarHasLeftRail(float progress) {
		return progress >= 1 || progress < 0;
	}

	public abstract RailController GetNextRail(CarController car);


	public float SpeedModAtProgress(float t) {
		float M = .01f;

		float distance = Vector3.Distance(GetPoint(t), GetPoint(t + M));
		if(distance == 0 || M*400/distance < 1) {
			distance = Vector3.Distance(GetPoint(t - M), GetPoint(t));
		}
		if (distance == 0) return 0;
		return M *400/ distance;
	}
}
