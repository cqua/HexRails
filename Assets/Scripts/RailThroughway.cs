using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class RailThroughway : Railway {
	RailExit Bottom, Top;

	public Railway ExitRailTop;
	public bool ExitRailTopToIntersection;
	public Railway ExitRailBottom;
	public bool ExitRailBottomToIntersection;

	protected override void InitializeExits() {
		Top = CreateExit(ExitRailTop, false, ExitRailTopToIntersection);
		Bottom = CreateExit(ExitRailBottom, false, ExitRailBottomToIntersection);
	}

	public override Vector3 GetPoint(float t, bool reverse = false) {
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

	public override Vector3 GetDirection(float t, bool reverse = false) {
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

	public override Railway GetNextRail(RailCar car) {
		float progress = car.progress;
		if(car.CurrentOrientation == Orientation.Reverse) {
			progress = Length - progress;
		}

		if (progress < 0) {
			car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			return Bottom.To;
		}

		if(progress >= Length) {
			car.CurrentOrientation = Top.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			return Top.To;
		}

		return this;
	}
}
