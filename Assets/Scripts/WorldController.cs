using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class WorldController : MonoBehaviour
{
    public static bool Paused = false;

	public RailCar Engine;
    public static SliderManager SpeedSlider;
    public static SwitchManager DirectionSwitch;

	public static WorldController Instance { get { return _instance; } }

	private static WorldController _instance;

    // Start is called before the first frame update
    void Start() {
		if(_instance == null) {
			_instance = this;
		} else {
			Destroy(this);
		}

        SpeedSlider = GameObject.FindGameObjectWithTag("SpeedSlider").GetComponent<SliderManager>();
        DirectionSwitch = GameObject.FindGameObjectWithTag("DirectionSwitch").GetComponent<SwitchManager>();

    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetButtonDown("Cancel")) {
			Debug.Log("Paused!");
			Paused = !Paused;

			if(Paused) {
				SpeedSlider.mainSlider.enabled = false;
				DirectionSwitch.switchButton.enabled = false;
			} else {
				SpeedSlider.mainSlider.enabled = true;
				DirectionSwitch.switchButton.enabled = true;
			}
		}

		if(!Paused) {
			float engineSpeed = 0f;

			switch (SpeedSlider.mainSlider.value) {
				case -1:
					engineSpeed = -.3f;
					break;
				case 1:
					engineSpeed = .3f;
					break;
				case 2:
					engineSpeed = .9f;
					break;
				case 3:
					engineSpeed = 1.8f;
					break;
				default:
					engineSpeed = 0;
					break;
			}

			Engine.Speed = engineSpeed * Engine.BaseSpeed;

			if (DirectionSwitch.isOn) {
				Engine.CurrentDirection = Direction.Right;
			} else {
				Engine.CurrentDirection = Direction.Left;
			}
		}
	}

	public static void ForceFullstop() {
		SpeedSlider.mainSlider.value = 0;
		Instance.Engine.Speed = 0;
	}
}
