using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class RailIntersection : Railway {
	RailExit Bottom, TopLeft, TopRight;

	public Railway ExitRailTopLeft;
	public bool ExitRailTopLeftToIntersection;
	public Railway ExitRailTopRight;
	public bool ExitRailTopRightToIntersection;
	public Railway ExitRailBottom;
	public bool ExitRailBottomToIntersection;

	protected override void InitializeExits() {
		TopLeft = CreateExit(ExitRailTopLeft, true, ExitRailTopLeftToIntersection);
		TopRight = CreateExit(ExitRailTopRight, true, ExitRailTopRightToIntersection);
		Bottom = CreateExit(ExitRailBottom, false, ExitRailBottomToIntersection);
	}

	public override Vector3 GetPoint(float t, bool reverse = false) {
		if (reverse)
			t = Length - t;

		if(t < 0) {
			if(Bottom.IsDeadEnd)
				return Points[0];
			return Bottom.To.GetPoint(t + Bottom.To.Length, (Bottom.FromAnIntersection || Bottom.ToAnIntersection) ? reverse : !reverse);
		}
		if (t >= Length) {
			return Points[Length];
		}

		return GetPointAlongLine(Points[Mathf.FloorToInt(t)], Points[Mathf.FloorToInt(t) + 1], t - Mathf.FloorToInt(t));
	}

	public override Railway GetNextRail(RailCar car) {
		float progress = car.progress;
		if(car.CurrentOrientation == Orientation.Reverse) {
			progress = Length - progress;
		}

		if (progress < 0) {
			if (Bottom.IsDeadEnd) {
				return null;
			}

			if (car.CurrentOrientation == Orientation.Forward) {
				car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
				car.progress += Bottom.To.Length;
			} else {
				car.progress -= Length;
				car.CurrentOrientation = Bottom.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
			}
			return Bottom.To;
		}

		if(progress >= Length) {
			if(car.CurrentOrientation == Orientation.Forward) {
				if (!TopLeft.IsDeadEnd && (car.CurrentDirection == Direction.Left || car.TargetConnection.Railway == TopLeft.To)) {
					car.progress -= Length;
					car.CurrentOrientation = TopLeft.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
					return TopLeft.To;

				} else if (!TopRight.IsDeadEnd && car.TargetConnection.Railway == TopRight.To) {
					car.progress -= Length;
					car.CurrentOrientation = TopRight.ToAnIntersection ? Orientation.Reverse : Orientation.Forward;
					return TopRight.To;

				} else {
					return null;
				}
			} else {
				if (!TopLeft.IsDeadEnd) {
					car.progress += TopLeft.To.Length;
					car.CurrentOrientation = TopLeft.ToAnIntersection ? Orientation.Forward : Orientation.Reverse;
					return TopLeft.To;
				} else if (!TopRight.IsDeadEnd) {
					car.progress += TopRight.To.Length;
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
