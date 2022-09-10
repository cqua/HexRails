using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;

	public bool IsEngine { get { return TargetConnection == null; } }

	public float BaseSpeed = 2f, Speed = 1;

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
		}
		else 
		{ 
			Speed = TargetConnection.Speed;
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

		progress += (Time.deltaTime) * Speed;

		if (Railway.CarHasLeftRail(progress) && Speed != 0) {

			var nextRail = Railway.GetNextRail(this);
			
			if(nextRail == null) {
				// problem with rail, automatic fullstop
				WorldController.ForceFullstop();

				if(CurrentOrientation == Orientation.Forward) {
					progress = (Time.deltaTime) * Speed + .01f;
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