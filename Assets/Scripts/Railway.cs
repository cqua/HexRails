using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class Railway : MonoBehaviour
{
	[SerializeField]
	//public List<BezierSpline> Rails;

	public int railModelFrequency = 30;

	public List<Vector3> Points;
	public int Length { get { return Points == null ? 0 : Points.Count - 1; } }

	RailExit Bottom, TopLeft, TopRight;

	public Railway ExitRailTopLeft;
	public bool ExitRailTopLeftToIntersection;
	public Railway ExitRailTopRight;
	public bool ExitRailTopRightToIntersection;
	public Railway ExitRailBottom;
	public bool ExitRailBottomToIntersection;

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

		TopLeft = CreateExit(ExitRailTopLeft, true, ExitRailTopLeftToIntersection);
		TopRight = CreateExit(ExitRailTopRight, true, ExitRailTopRightToIntersection);
		Bottom = CreateExit(ExitRailBottom, false, ExitRailBottomToIntersection);
	}

	RailExit CreateExit(Railway to, bool fromIntersection, bool toIntersection) {
		if(to == null) {
			return new RailExit();
		}
		return new RailExit(this, to, fromIntersection, toIntersection);
	}

	//public Line GetSpline(float t) {
	//	int index = 0;

	//	while(t >= 1) {
	//		t -= 1f;
	//		index += 1;
	//	}
	//	while (t < 0) {
	//		t += 1f;
	//		index -= 1;
	//	}

	//	while (index < 0) {
	//		index += Length;
	//	}

	//	return new Line(Points[index % Length], Points[++index % Length]);
	//}

	public Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = Length - t;

		if(t < 0) {
			return Bottom.To.GetPoint(t + Bottom.To.Length, (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
		}
		if (t >= Length) {
			return Points[Length];
		}

		return GetPointAlongLine(Points[Mathf.FloorToInt(t)], Points[Mathf.FloorToInt(t) + 1], t - Mathf.FloorToInt(t));
	}

	public Vector3 GetDirection(float t, bool reverse = false) {
		if (reverse)
			t = t - 1;

		if (t < 0) {
			return Bottom.To.GetDirection(t + Bottom.To.Length, (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
		}
		if (t >= Length) {
			return transform.position + Points[Length];
		}

		return transform.position + Points[Mathf.FloorToInt(t) + 1];
	}

	private Vector3 GetPointAlongLine(Vector3 p0, Vector3 p1, float progress) {
		float x = p0.x * (1 - progress) + p1.x * progress;
		float y = p0.y * (1 - progress) + p1.y * progress;
		float z = p0.z * (1 - progress) + p1.z * progress;

		return transform.position + new Vector3(x, y, z);
	}

	public bool CarHasLeftRail(float progress) {
		return progress >= Length || progress < 0;
	}

	public Railway GetNextRail(RailCar car) {
		float progress = car.progress;
		if(car.CurrentOrientation == Orientation.Reverse) {
			progress = Length - progress;
		}

		if (progress < 0) {
			car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			return Bottom.To;
		}

		if(progress >= Length) {
			if(car.CurrentDirection == Direction.Left) {
				car.CurrentOrientation = TopLeft.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
				return TopLeft.To;
			} else {
				car.CurrentOrientation = TopRight.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
				return TopRight.To;
			}
		}

		return this;
	}
}
