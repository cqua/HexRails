using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class RailIntersectionController : RailController {
	RailExit Bottom, TopLeft, TopRight;

	public RailController Entrance;
	public bool EntranceFromExit;
	public RailController LeftExit;
	public bool LeftExitToExit;
	public RailController RightExit;
	public bool RightExitToExit;

	protected override void InitializeExits() {
		TopLeft = CreateExit(LeftExit, true, LeftExitToExit);
		TopRight = CreateExit(RightExit, true, RightExitToExit);
		Bottom = CreateExit(Entrance, false, EntranceFromExit);
	}

	public override Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = 1 - t;

		if(float.IsNaN(t)) {
			return Vector3.zero;
		}

		if (float.IsInfinity(t)) {
			return Vector3.zero;
		}

		if (t < 0) {
			if(Bottom != null && !Bottom.IsDeadEnd)
				return Bottom.To.GetPoint(Mathf.CeilToInt(t) - t, (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
			t = 0;
		}
		if (t >= 1) {
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
			if (Bottom == null || Bottom.IsDeadEnd) {
				return null;
			}

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
				if (!TopLeft.IsDeadEnd && (car.CurrentDirection == Direction.Left ||
					(!car.IsEngine && car.TargetConnection.Railway == TopLeft.To))) {
					car.progress -= 1;
					car.CurrentOrientation = TopLeft.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
					return TopLeft.To;

				} else if (!TopRight.IsDeadEnd) {
					car.progress -= 1;
					car.CurrentOrientation = TopRight.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
					return TopRight.To;

				} else {
					return null;
				}
			} else {
				if (!TopLeft.IsDeadEnd) {
					car.progress += 1;
					car.CurrentOrientation = TopLeft.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
					return TopLeft.To;
				} else if (!TopRight.IsDeadEnd) {
					car.progress += 1;
					car.CurrentOrientation = TopRight.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
					return TopRight.To;
				} else {
					return null;
				}
			}
		}

		return this;
	}
}
