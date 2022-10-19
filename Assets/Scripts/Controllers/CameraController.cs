using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    CarController PlayerEngine;
    float CameraMaxDistance = 30f;
	public float oX, oY, oZ, oW;

	public float CamAngle = 180f;
	float Sensitivity = 1f;
	public float CamHeight = 10f;

    // Start is called before the first frame update
    void Start() {
        PlayerEngine = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update() {
		if (WorldController.Paused) return;

        Vector3 ppos = PlayerEngine.transform.position;

		CamAngle = CamAngle + Input.GetAxis("Mouse X") * -10f * Sensitivity;

		float deltaY = Input.GetAxis("Mouse Y") * -1 * Sensitivity;
		if ((CamHeight < 18 && deltaY > 0) || (CamHeight > 2 && deltaY < 0)) {
			if(CamHeight + deltaY > 18) {
				CamHeight = 18;
			} else if (CamHeight + deltaY < 2) {
				CamHeight = 2;
			} else {
				CamHeight += deltaY;
			}
		}

		//if (PlayerEngine.Speed >= 0) {
		//	TargetAngle = 270 - PlayerEngine.transform.rotation.eulerAngles.y;
		//} else if (PlayerEngine.Speed < 0) {
		//	TargetAngle = 90 - PlayerEngine.transform.rotation.eulerAngles.y;
		//} else TargetAngle = CamAngle;

		//if(TargetAngle > 180) {
		//	TargetAngle -= 360;
		//} else if(TargetAngle < -180) {
		//	TargetAngle -= 360;
		//}

		//if (CamAngle > 180) {
		//	CamAngle -= 360;
		//} else if (CamAngle < -180) {
		//	CamAngle -= 360;
		//}

		//float frameSpeed = Speed * Time.deltaTime;// * Mathf.Abs(CamAngle - TargetAngle) / TargetAngle;

		//if (CamAngle + frameSpeed < TargetAngle) {
		//	CamAngle += frameSpeed;
		//} else if (CamAngle - frameSpeed > TargetAngle) {
		//	CamAngle -= frameSpeed;
		//}

		//CamAngle = 270 - PlayerEngine.transform.rotation.eulerAngles.y;

		transform.position = new Vector3(
			ppos.x + CameraMaxDistance * Mathf.Cos(CamAngle * Mathf.PI / 180f), CamHeight,
			ppos.z + CameraMaxDistance * Mathf.Sin(CamAngle * Mathf.PI / 180f));

		//oX = CamAngle;
		//oY = TargetAngle;
		//oZ = PlayerEngine.transform.rotation.eulerAngles.z;
		//oW = PlayerEngine.transform.rotation.w * 180 / Mathf.PI;

		transform.LookAt(PlayerEngine.transform);

		transform.position = new Vector3(
			transform.position.x, CamHeight + 5,
			transform.position.z);
		//transform.rotation = new Quaternion(oX != -1 ? oX : PlayerEngine.transform.rotation.x,
		//	oY != -1 ? oY : PlayerEngine.transform.rotation.y,
		//	oZ != -1 ? oZ : PlayerEngine.transform.rotation.z,
		//	oW != -1 ? oW : PlayerEngine.transform.rotation.w);


		//float nextx = cpos.x, nextz = cpos.z;
		//if (cpos.x < ppos.x - CameraMaxDistance) {
		//	nextx = ppos.x - CameraMaxDistance;
		//} else if (cpos.x > ppos.x + CameraMaxDistance) {
		//	nextx = ppos.x + CameraMaxDistance;
		//}

		//if (cpos.z < ppos.z - CameraMaxDistance) {
		//	nextz = ppos.z - CameraMaxDistance;
		//} else if (cpos.z > ppos.z + CameraMaxDistance) {
		//	nextz = ppos.z + CameraMaxDistance;
		//}

		//transform.position = new Vector3(nextx, ppos.y + 30, nextz);
	}
}
