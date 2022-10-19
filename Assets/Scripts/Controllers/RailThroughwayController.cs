using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class RailThroughwayController : RailController {
	RailExit Bottom, Top;

	public RailController ExitRailTop;
	public bool ExitRailTopToIntersection;
	public RailController ExitRailBottom;
	public bool ExitRailBottomToIntersection;

	protected override void InitializeExits() {
		Top = CreateExit(ExitRailTop, false, ExitRailTopToIntersection);
		Bottom = CreateExit(ExitRailBottom, false, ExitRailBottomToIntersection);
	}

	public override Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = 1 - t;

		if(float.IsNaN(t)) {
			return transform.position;
		}

		if (float.IsInfinity(t)) {
			return transform.position;
		}

		if (t < 0) {
			if(Bottom != null)
				return Bottom.To.GetPoint(Mathf.CeilToInt(t) - t, (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
			t = 0;
		}
		if (t >= 1) {
			if (Top != null)
				return Top.To.GetPoint(t - Mathf.FloorToInt(t), (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
			t = 1;
		}

		return Spline.GetPoint(t) + transform.position;
	}

	public override RailController GetNextRail(CarController car) {
		float progress = car.progress;
		if(car.CurrentOrientation == Orientation.Reverse) {
			progress = 1 - progress;
		}

		if (progress < 0) {
			if (car.CurrentOrientation == Orientation.Forward) {
				car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
				car.progress += 1;
			} else {
				car.progress -= 1;
				car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			}
			return Bottom.To;
		}

		if (progress >= 1) {
			if (car.CurrentOrientation == Orientation.Forward) {
				car.progress -= 1;
				car.CurrentOrientation = Top.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			} else {
				car.progress += 1;
				car.CurrentOrientation = Top.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
			}
			return Top.To;
		}

		return this;
	}
}
