using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railway : MonoBehaviour
{
	[SerializeField]
	public List<BezierSpline> Rails;
	public int Length { get { return Rails == null ? 0 : Rails.Count; } }

	public int railModelFrequency;

	public bool lookForward;

	private void Awake() {
		Object railPrefab = Resources.Load<Object>("Prefabs/Rail");

		if(railPrefab == null) {
			Debug.LogError("Failed to load Rail prefab.");
		} else {
			foreach (var spline in Rails) {
				for (int p = 0, f = 0; f < railModelFrequency; f++, p++) {
					GameObject item = Instantiate(railPrefab) as GameObject;
					float progress = (float)p / (float)railModelFrequency;
					Vector3 position = spline.GetPoint(progress);
					item.transform.localPosition = position;
					if (lookForward) {
						item.transform.LookAt(position + spline.GetDirection(progress));
					}
					item.transform.parent = spline.transform;
					Debug.Log("Created Rail at progress " + p / railModelFrequency + ".");
				}
			}
		}
	}

	public BezierSpline GetSpline(float t) {
		int index = 0;

		while(t >= 1) {
			t -= 1f;
			index += 1;
		}
		while (t < 0) {
			t += 1f;
			index -= 1;
		}

		while (index < 0) {
			index += Rails.Count;
		}

		return Rails[index % Rails.Count];
	}

	public Vector3 GetPoint(float t) {
		return GetSpline(t).GetPoint(t - Mathf.Floor(t));
	}

	public Vector3 GetDirection(float t) {
		return GetSpline(t).GetVelocity(t - Mathf.Floor(t)).normalized;
	}
}
