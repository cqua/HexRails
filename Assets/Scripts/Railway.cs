using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public abstract class Railway : MonoBehaviour {
	public int railModelFrequency = 30;

	public List<Vector3> Points;
	public int Length { get { return Points == null ? 0 : Points.Count - 1; } }

	private void Awake() {
		Object railPrefab = Resources.Load<Object>("Prefabs/Rail");

		if(railPrefab == null) {
			Debug.LogError("Failed to load Rail prefab.");
		} else {
			for(int i = 0; i < Length; i++) {
				for (int p = 0, f = 0; f < railModelFrequency; f++, p++) {
					GameObject item = Instantiate(railPrefab) as GameObject;
					float progress = (float)p / (float)railModelFrequency;
					Vector3 position = GetPointAlongLine(Points[i], Points[i + 1], progress);
					item.transform.localPosition = position;
					item.transform.LookAt(GetDirection(i + progress));
					item.transform.parent = this.transform;
				}
			}
		}

		InitializeExits();
	}

	protected abstract void InitializeExits();

	protected virtual RailExit CreateExit(Railway to, bool fromIntersection, bool toIntersection) {
		if(to == null) {
			return new RailExit();
		}
		return new RailExit(this, to, fromIntersection, toIntersection);
	}

	public virtual Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = Length - t;
		
		if (t >= Length) {
			return Points[Length];
		}

		return GetPointAlongLine(Points[Mathf.FloorToInt(t)], Points[Mathf.FloorToInt(t) + 1], t - Mathf.FloorToInt(t));
	}

	public virtual Vector3 GetDirection(float t, bool reverse = false) {
		if (reverse)
			t = t - 1;
		
		if (t >= Length) {
			return transform.position + Points[Length];
		}

		return transform.position + Points[Mathf.FloorToInt(t) + 1];
	}

	protected Vector3 GetPointAlongLine(Vector3 p0, Vector3 p1, float progress) {
		float x = p0.x * (1 - progress) + p1.x * progress;
		float y = p0.y * (1 - progress) + p1.y * progress;
		float z = p0.z * (1 - progress) + p1.z * progress;

		return transform.position + new Vector3(x, y, z);
	}

	public bool CarHasLeftRail(float progress) {
		return progress >= Length || progress < 0;
	}

	public abstract Railway GetNextRail(RailCar car);
}
