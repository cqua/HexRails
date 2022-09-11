using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;

	public bool IsEngine { get { return TargetConnection == null; } }

	public float BaseSpeed = 2f, Velocity = 0f, TargetVelocity = 0f, Acceleration = 0f;
	public float InitialVelocity = 0f;

	public Direction CurrentDirection = Direction.Left;
	public Orientation CurrentOrientation = Orientation.Forward;

	public float progress = 0;

	public RailCar TargetConnection;

	public float SpaceBetwixtCars = .22f;

	public float TimeSinceVelocityWasSet = 0f;

	public float AccelerationModifier = 3f;

	private void Awake() {
		if(TargetConnection != null) {
			Railway = TargetConnection.Railway;
			progress = TargetConnection.progress - SpaceBetwixtCars;
		}
	}

	private void Update () {
		if (WorldController.Paused) return;

		if (TargetConnection == null) {

			if(TimeSinceVelocityWasSet < 1) {
				//VELOCITY EQUATION
				//(3.6x^2 - 3.7x^3 + 1.1x^4) * (TargetVelocity - Initial Velocity) + Initial Velocity

				TimeSinceVelocityWasSet += Time.deltaTime / AccelerationModifier;

				float x = TimeSinceVelocityWasSet;
				Velocity = (3.6f * Mathf.Pow(x, 2)
					- 3.7f* Mathf.Pow(x, 3)
					+ 1.1f * Mathf.Pow(x, 4))
					* (TargetVelocity - InitialVelocity) + InitialVelocity;
			} else {
				Velocity = TargetVelocity;
			}
		}
		else 
		{
			Velocity = TargetConnection.Velocity;
			CurrentDirection = TargetConnection.CurrentDirection;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			//progress = TargetConnection.progress - SpaceBetwixtCars;
			if (Railway == TargetConnection.Railway) {
				CurrentOrientation = TargetConnection.CurrentOrientation;

				if (progress > TargetConnection.progress - SpaceBetwixtCars) {
					progress = TargetConnection.progress - SpaceBetwixtCars;
				}
				if (progress < TargetConnection.progress - SpaceBetwixtCars) {
					progress = TargetConnection.progress - SpaceBetwixtCars;
				}
			}
		}

		progress += (Time.deltaTime) * Velocity;

		if (Railway.CarHasLeftRail(progress) && Velocity != 0) {

			var nextRail = Railway.GetNextRail(this);
			
			if(nextRail == null) {
				// problem with rail, automatic fullstop
				WorldController.ForceFullstop();

				if(CurrentOrientation == Orientation.Forward) {
					progress = (Time.deltaTime) * Velocity + .01f;
				} else {
					progress = Railway.Length;
				}
			} else {
				// continue onto rail
				Railway = nextRail;
			}
		}

		Vector3 position = Railway.GetPoint(progress, CurrentOrientation == Orientation.Reverse);
		transform.localPosition = position;
		transform.LookAt(Railway.GetDirection(progress, CurrentOrientation == Orientation.Reverse));
	}

	public void SetTargetVelocity(float nv) {
		if (TargetVelocity != nv) {
			TargetVelocity = nv;
			InitialVelocity = Velocity;
			TimeSinceVelocityWasSet = 0f;

			if(TargetVelocity < 0 && InitialVelocity > 0) {
				AccelerationModifier = 2f;
			} else {
				AccelerationModifier = 3f;
			}
		}
	}
}