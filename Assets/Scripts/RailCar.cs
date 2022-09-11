using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;

	public bool IsEngine { get { return TargetConnection == null; } }

	public float BaseSpeed = 2f, Velocity = 1, TargetVelocity = 1;

	public Direction CurrentDirection = Direction.Left;
	public Orientation CurrentOrientation = Orientation.Forward;

	public float progress = 0;

	public RailCar TargetConnection;

	public float SpaceBetwixtCars = .22f;

	private void Awake() {
		if(TargetConnection != null) {
			Railway = TargetConnection.Railway;
			progress = TargetConnection.progress - SpaceBetwixtCars;
		}
	}

	private void Update () {
		if (WorldController.Paused) return;

		if (TargetConnection == null) {
			if(Velocity != TargetVelocity) {
				if(Mathf.Abs(Velocity - TargetVelocity) < .0001f) {
					Velocity = TargetVelocity;
				} else {
					Velocity = Velocity + (TargetVelocity - Velocity) / 16f;
				}
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
}