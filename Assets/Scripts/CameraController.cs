using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    RailCar PlayerEngine;
    float CameraSpeed = .5f;
    float CameraMaxDistance = 30f;

    // Start is called before the first frame update
    void Start() {
        PlayerEngine = GameObject.FindGameObjectWithTag("Player").GetComponent<RailCar>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 cpos = transform.position;
        Vector3 ppos = PlayerEngine.transform.position;

        float nextx = cpos.x, nextz = cpos.z;
        if(cpos.x < ppos.x - CameraMaxDistance) {
            nextx = ppos.x - CameraMaxDistance;
		} else if (cpos.x > ppos.x + CameraMaxDistance) {
            nextx = ppos.x + CameraMaxDistance;
        }

        if (cpos.z < ppos.z - CameraMaxDistance) {
            nextz = ppos.z - CameraMaxDistance;
        } else if (cpos.z > ppos.z + CameraMaxDistance) {
            nextz = ppos.z + CameraMaxDistance;
        }

        transform.position = new Vector3(nextx, ppos.y + 30, nextz);
    }
}
