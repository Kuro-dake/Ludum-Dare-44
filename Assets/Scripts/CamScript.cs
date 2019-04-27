using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {

	Vector3 orig_pos;

	void Start(){
		orig_pos = transform.position;
	}

	public void Shake(){
		GM.routines.CStart ("shake_screen", ShakeRoutine ());
	}

	IEnumerator ShakeRoutine(){
		float duration = Setup.base_settings.GetFloat ("shake_screen_duration");
		while (duration > 0f) {
			duration -= Time.deltaTime;
			transform.position = orig_pos + Random.insideUnitSphere * Setup.base_settings.GetFloat("shake_screen_force");
			yield return null;
		}
		transform.position = orig_pos;
	}
}
