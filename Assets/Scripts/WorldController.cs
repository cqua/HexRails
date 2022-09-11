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

	public GameObject PausePanel;

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

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

    // Update is called once per frame
    void Update() {
		if (Input.GetButtonDown("Cancel")) {
			if (Paused) {
				Unpause();
			} else {
				Pause();
			}
		}

		if(!Paused) {
			float engineSpeed = 0f;

			if(Input.GetKeyDown(KeyCode.W) && SpeedSlider.mainSlider.value < 3) {
				SpeedSlider.mainSlider.value++;
			} else if (Input.GetKeyDown(KeyCode.S) && SpeedSlider.mainSlider.value > -1) {
				SpeedSlider.mainSlider.value--;
			}

			if (Input.GetKeyDown(KeyCode.A) && DirectionSwitch.isOn) {
				DirectionSwitch.AnimateSwitch();
				DirectionSwitch.isOn = false;
			} else if (Input.GetKeyDown(KeyCode.D) && !DirectionSwitch.isOn) {
				DirectionSwitch.AnimateSwitch();
				DirectionSwitch.isOn = true;
			}

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

			Engine.TargetVelocity = engineSpeed * Engine.BaseSpeed;

			if (DirectionSwitch.isOn) {
				Engine.CurrentDirection = Direction.Right;
			} else {
				Engine.CurrentDirection = Direction.Left;
			}
		}
	}

	public static void ForceFullstop() {
		SpeedSlider.mainSlider.value = 0;
		Instance.Engine.Velocity = 0;
	}

	public void Pause() {
		SpeedSlider.mainSlider.enabled = false;
		DirectionSwitch.switchButton.enabled = false;

		PausePanel.SetActive(true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Paused = true;
	}

	public void Unpause() {
		SpeedSlider.mainSlider.enabled = true;
		DirectionSwitch.switchButton.enabled = true;

		PausePanel.SetActive(false);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Paused = false;
	}

	public void ExitGame() {
		Application.Quit();
	}
}
