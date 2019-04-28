using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour {

	public GameObject heart;

	// Use this for initialization
	protected virtual void Start () {
		
	}
	public virtual void Initialize(Character to_follow){
		transform.DestroyChildren ();
	}
	public virtual void DisplayHP(int hp){
		transform.DestroyChildren ();
		float margin = Mathf.Clamp(300f/hp, 0f, 35f);
		for (int i = 0; i < hp; i++) {
			GameObject h = GameObject.Instantiate (heart);
			h.transform.SetParent (transform, false);
			h.GetComponent<RectTransform> ().anchoredPosition += Vector2.right * margin * (i);
		}
	}
}
