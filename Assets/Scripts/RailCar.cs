using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;
	
	public SliderManager SpeedSlider;
	public SwitchManager DirectionSwitch;

	public float Speed = .05f;

	public Direction CurrentDirection = Direction.Left;
	public Orientation CurrentOrientation = Orientation.Forward;

	public float progress;

	public RailCar TargetConnection;

	public float SpaceBetwixtCars = .11f;

	private void Awake() {
		if(TargetConnection != null) {
			Railway = TargetConnection.Railway;
			progress = TargetConnection.progress - SpaceBetwixtCars;
		}

		SpeedSlider = GameObject.FindGameObjectWithTag("SpeedSlider").GetComponent<SliderManager>();
		DirectionSwitch = GameObject.FindGameObjectWithTag("DirectionSwitch").GetComponent<SwitchManager>();
	}

	private void Update () {
		if (TargetConnection == null) {
			switch (SpeedSlider.mainSlider.value) {
				case -1:
					Speed = -.05f;
					break;
				case 1:
					Speed = .05f;
					break;
				case 2:
					Speed = .3f;
					break;
				case 3:
					Speed = 1.8f;
					break;
				default:
					Speed = 0;
					break;
			}

			if (DirectionSwitch.isOn) {
				CurrentDirection = Direction.Right;
			} else {
				CurrentDirection = Direction.Left;
			}
		}
		else 
		{ 
			Speed = TargetConnection.Speed;
			CurrentDirection = TargetConnection.CurrentDirection;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			//progress = TargetConnection.progress - SpaceBetwixtCars;
			if (Railway == TargetConnection.Railway) {
				CurrentOrientation = TargetConnection.CurrentOrientation;

				int directionMod = CurrentOrientation == Orientation.Forward ? 1 : -1;

				if (progress > TargetConnection.progress - SpaceBetwixtCars * directionMod) {
					progress = TargetConnection.progress - SpaceBetwixtCars * directionMod;
				}
				if (progress < TargetConnection.progress + (- SpaceBetwixtCars - .02f) * directionMod) {
					progress = TargetConnection.progress + (- SpaceBetwixtCars - .02f) * directionMod;
				}
			}
		}

		progress += (Time.deltaTime) * Speed;
		
		if(Railway.CarHasLeftRail(progress)) {

			var nextRail = Railway.GetNextRail(this);

			if (CurrentOrientation == Orientation.Forward) {
				progress -= Railway.Length;
			}

			Railway = nextRail;

			if (CurrentOrientation == Orientation.Reverse) {
				progress += Railway.Length;
			}
		}

		Vector3 position = Railway.GetPoint(progress, CurrentOrientation == Orientation.Reverse);
		transform.localPosition = position;
		transform.LookAt(Railway.GetDirection(progress, CurrentOrientation == Orientation.Reverse));
	}
}