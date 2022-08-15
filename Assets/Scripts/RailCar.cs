using UnityEngine;

public class RailCar : MonoBehaviour {

	public Railway Railway;

	public float Speed = .05f;

	public bool lookForward;

	public ProgressMode mode;

	public float progress;
	private bool goingForward = true;

	public RailCar TargetConnection;

	public float SpaceBetwixtCars = .11f;

	private void Awake() {
		if(TargetConnection != null) {
			Railway = TargetConnection.Railway;
			progress = TargetConnection.progress - SpaceBetwixtCars;
		}
	}

	private void Update () {

		if (TargetConnection != null) {
			Speed = TargetConnection.Speed;
			goingForward = TargetConnection.goingForward;
			SpaceBetwixtCars = TargetConnection.SpaceBetwixtCars;
			//progress = TargetConnection.progress - SpaceBetwixtCars;
			if(progress > TargetConnection.progress - SpaceBetwixtCars) {
				progress = TargetConnection.progress - SpaceBetwixtCars;
			}
			if (progress < TargetConnection.progress - SpaceBetwixtCars - .02f) {
				progress = TargetConnection.progress - SpaceBetwixtCars - .02f;
			}
		}

			if (goingForward) {
				progress += (Time.deltaTime) * Speed;
				if (progress >= Railway.Length) {
					if (mode == ProgressMode.Once) {
						progress = Railway.Length;
					} else if (mode == ProgressMode.Loop) {
						progress -= Railway.Length;
					} else {
						progress = Railway.Length;
						goingForward = false;
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