using UnityEngine;
using Michsky.UI.ModernUIPack;

public class CarController : MonoBehaviour {

	public RailController Railway;

	public bool IsEngine { get { return TargetConnection == null; } }

	public float BaseSpeed = .2f, Velocity = 0f, TargetVelocity = 0f;
	public float VCurveStart = 0f;

	public Direction CurrentDirection = Direction.Left;
	public Orientation CurrentOrientation = Orientation.Forward;

	public float progress = 0.5f;

	public CarController TargetConnection;

	public float SpaceBetwixtCars = .009f;

	public float TimeSinceVelocityWasSet = 0f;

	public float AccelerationMod = 3f;
	public float SpeedMod;

	private void Awake() {
		if(!IsEngine) {
			Railway = TargetConnection.Railway;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			progress = TargetConnection.progress - SpaceBetwixtCars;
		}
	}

	private void Update () {
		if (WorldController.Paused) return;

		//update progress
		if (IsEngine) {

			if (TimeSinceVelocityWasSet < 1) {
				//VELOCITY EQUATION
				//(3.6x^2 - 3.7x^3 + 1.1x^4) * (TargetVelocity - Initial Velocity) + Initial Velocity

				TimeSinceVelocityWasSet += Time.deltaTime / AccelerationMod;

				float x = TimeSinceVelocityWasSet;
				Velocity = (3.6f * Mathf.Pow(x, 2)
					- 3.7f * Mathf.Pow(x, 3)
					+ 1.1f * Mathf.Pow(x, 4))
					* (TargetVelocity - VCurveStart) + VCurveStart;
			} else {
				Velocity = TargetVelocity;
			}
			SpeedMod = Railway.SpeedModAtProgress(progress);
			progress += (Time.deltaTime) * Velocity * SpeedMod;
		} else {
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			Velocity = TargetConnection.Velocity;
			SpeedMod = Railway.SpeedModAtProgress(progress);
			CurrentDirection = TargetConnection.CurrentDirection;

			if (Railway == TargetConnection.Railway) {
				CurrentOrientation = TargetConnection.CurrentOrientation;

				progress = (TargetConnection.progress - SpaceBetwixtCars * SpeedMod);
			} else {
				progress += (Time.deltaTime) * Velocity * SpeedMod;
			}
		}

		//Update Railway
		if (Railway.CarHasLeftRail(progress) && Velocity != 0) {

			var nextRail = Railway.GetNextRail(this);
			
			if(nextRail == null) {
				// problem with rail, automatic fullstop
				WorldController.ForceFullstop();

				if(CurrentOrientation == Orientation.Forward) {
					progress = (Time.deltaTime) * Velocity * SpeedMod;
				} else {
					progress = 1  - (Time.deltaTime) * Velocity * SpeedMod;
				}
			} else {
				// continue onto rail
				Railway = nextRail;
			}
		}

		//update position
		Vector3 position = Railway.GetPoint(progress, CurrentOrientation == Orientation.Reverse);
		transform.localPosition = position;
		
		//update orientation
		transform.LookAt(Railway.GetDirection(progress, CurrentOrientation == Orientation.Reverse));
	}

	public void SetTargetVelocity(float nv) {
		if (TargetVelocity != nv) {
			TargetVelocity = nv;
			VCurveStart = Velocity;
			TimeSinceVelocityWasSet = 0f;

			if(TargetVelocity < 0 && VCurveStart > 0) {
				AccelerationMod = 2f;
			} else {
				AccelerationMod = 3f;
			}
		}
	}
}