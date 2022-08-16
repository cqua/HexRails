using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;
	
	public SliderManager SpeedSlider;
	public SwitchManager DirectionSwitch;

	public float Speed = .05f;

	public bool lookForward;

	public ProgressMode mode;

	public float progress;
	private bool goingForward = true;
	public bool goingLeft = true;

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
		switch(SpeedSlider.mainSlider.value) {
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

		goingLeft = !DirectionSwitch.isOn;

		if (TargetConnection != null) {
			Speed = TargetConnection.Speed;
			goingForward = TargetConnection.goingForward;
			goingLeft = TargetConnection.goingLeft;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			//progress = TargetConnection.progress - SpaceBetwixtCars;
			if (Railway == TargetConnection.Railway) {
				if (progress > TargetConnection.progress - SpaceBetwixtCars) {
					progress = TargetConnection.progress - SpaceBetwixtCars;
				}
				if (progress < TargetConnection.progress - SpaceBetwixtCars - .02f) {
					progress = TargetConnection.progress - SpaceBetwixtCars - .02f;
				}
			}
		}

		if (goingForward) {
			progress += (Time.deltaTime) * Speed;
			if (progress >= Railway.Length) {
				if (goingLeft) {
					if (!Railway.LoopLeft) {
						Railway = Railway.ExitLeft;
					} else {
						progress -= Railway.Length;
					}
				} else {
					if (!Railway.LoopRight) {
						Railway = Railway.ExitRight;
					} else {
						progress -= Railway.Length;
					}
				}
			}
		} else {
			if (Railway == null) {
				Railway = TargetConnection.Railway;
			}

			progress -= Time.deltaTime * Speed;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}

		Vector3 position = Railway.GetPoint(progress);
		transform.localPosition = position;
		if (lookForward) {
			transform.LookAt(position + Railway.GetDirection(progress));
		}
	}
}