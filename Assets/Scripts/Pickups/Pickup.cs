using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour {

	public GridPosition gp;
	protected bool active = false;
	public abstract void Take();
	public void Place(GridPosition gp_place){
		gp = gp_place;
		transform.position = gp.ft.transform.position;
		active = true;
	}
	protected void FadeOut(){
		StartCoroutine (FadeOutStep ());
	}
	IEnumerator FadeOutStep(){
		while (transform.sr ().color.a > 0f) {
			transform.sr ().color -= Color.black * Time.deltaTime;
			transform.localScale += Vector3.one * Time.deltaTime * .4f;
			yield return null;
		}
		Destroy (gameObject);
	}
}
