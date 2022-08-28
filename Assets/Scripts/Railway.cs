using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : MonoBehaviour
{
	[SerializeField]
	//public List<BezierSpline> Rails;

	public List<Vector3> Points;
	public int Length { get { return Points == null ? 0 : Points.Count - 1; } }

	public int railModelFrequency;

	public bool LoopRight, LoopLeft;
	public bool ReverseRight, ReverseLeft;
	public Railway ExitRight, ExitLeft;
	public Railway Previous;

	private void Awake() {
		Object railPrefab = Resources.Load<Object>("Prefabs/Rail");

		if(railPrefab == null) {
			Debug.LogError("Failed to load Rail prefab.");
		} else {
			for(int i = 0; i < Length; i++) {
				for (int p = 0, f = 0; f < railModelFrequency; f++, p++) {
					GameObject item = Instantiate(railPrefab) as GameObject;
					float progress = (float)p / (float)railModelFrequency;
					Vector3 position = GetPointAlongLine(Points[i], Points[i + 1], progress);
					item.transform.localPosition = position;
					item.transform.LookAt(GetDirection(i + progress));
					item.transform.parent = this.transform;
				}
			}
		}
	}

	//public Line GetSpline(float t) {
	//	int index = 0;

	//	while(t >= 1) {
	//		t -= 1f;
	//		index += 1;
	//	}
	//	while (t < 0) {
	//		t += 1f;
	//		index -= 1;
	//	}

	//	while (index < 0) {
	//		index += Length;
	//	}

	//	return new Line(Points[index % Length], Points[++index % Length]);
	//}

	public Vector3 GetPoint(float t) {
		if(t < 0) {
			return Previous.GetPoint(t + Previous.Length);
		}
		if (t >= Length) {
			return Points[Length];
		}
		Debug.Log(t);
		return GetPointAlongLine(Points[Mathf.FloorToInt(t)], Points[Mathf.FloorToInt(t) + 1], t - Mathf.FloorToInt(t));
	}

	public Vector3 GetDirection(float t) {
		if (t < 0) {
			return Previous.GetPoint(t + Previous.Length);
		}
		Debug.Log(Mathf.FloorToInt(t));

		return transform.position + Points[Mathf.FloorToInt(t) + 1];
	}

	private Vector3 GetPointAlongLine(Vector3 p0, Vector3 p1, float progress) {
		float x = p0.x * (1 - progress) + p1.x * progress;
		float y = p0.y * (1 - progress) + p1.y * progress;
		float z = p0.z * (1 - progress) + p1.z * progress;

		return transform.position + new Vector3(x, y, z);
	}
}
