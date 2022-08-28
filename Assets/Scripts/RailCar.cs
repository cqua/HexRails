using UnityEngine;
using Michsky.UI.ModernUIPack;

public class RailCar : MonoBehaviour {

	public Railway Railway;
	
	public SliderManager SpeedSlider;
	public SwitchManager DirectionSwitch;

	public float Speed = .05f;

	public bool lookForward = true;

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
			goingLeft = TargetConnection.goingLeft;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			//progress = TargetConnection.progress - SpaceBetwixtCars;
			if (Railway == TargetConnection.Railway) {
				goingForward = TargetConnection.goingForward;

				int dir = goingForward ? 1 : -1;

				if (progress > TargetConnection.progress - SpaceBetwixtCars * dir) {
					progress = TargetConnection.progress - SpaceBetwixtCars * dir;
				}
				if (progress < TargetConnection.progress + (- SpaceBetwixtCars - .02f) * dir) {
					progress = TargetConnection.progress + (- SpaceBetwixtCars - .02f) * dir;
				}
			}
		}

		if (goingForward) {
			progress += (Time.deltaTime) * Speed * (lookForward ? 1 : -1);

			if (progress >= Railway.Length || progress < 0) {
				if ((lookForward && Speed > 0) || (!lookForward && Speed < 0)) {
					if (goingLeft) {
						if (!Railway.LoopLeft) {
							lookForward = !Railway.ReverseLeft;
							Railway = Railway.ExitLeft;
						}
					} else {
						if (!Railway.LoopRight) {
							lookForward = !Railway.ReverseRight;
							Railway = Railway.ExitRight;
						}
					}
				} else {
					if (goingLeft) {
						if (!Railway.LoopRight) {
							if (Railway != Railway.Previous.ExitRight) {
								lookForward = !Railway.Previous.ReverseRight;
								Railway = Railway.Previous.ExitRight;
							} else {
								lookForward = !Railway.Previous.;
								Railway = Railway.Previous;
							}
						}
					} else {
						if (!Railway.LoopLeft) {
							if (Railway != Railway.Previous.ExitLeft) {
								lookForward = !Railway.Previous.ReverseLeft;
								Railway = Railway.Previous.ExitLeft;
							} else {
								Railway = Railway.Previous;
							}
						}
					}
				}
				if(lookForward) {
					progress = 0;
				} else {
					progress = Railway.Length - .001f;
				}
			}
		}

		Vector3 position = Railway.GetPoint(progress);
		transform.localPosition = position;
		if (lookForward) {
			transform.LookAt(Railway.GetDirection(progress));
		} else {
			transform.LookAt(Railway.GetDirection(progress - 1));
		}
	}
}