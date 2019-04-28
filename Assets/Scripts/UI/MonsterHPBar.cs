using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHPBar : HPBar {

	Character follow;
	public int disp;
	public override void DisplayHP (int hp)
	{
		disp = hp;
		transform.DestroyChildren ();
		float margin = Mathf.Clamp(6f/hp, 0f,1.64f);
		for (int i = 0; i < hp; i++) {
			GameObject h = GameObject.Instantiate (heart);

			h.transform.SetParent (transform, false);
			h.transform.localPosition += Vector3.up * margin * i;
			h.sr ().sortingOrder = 100 -i;

		}
	}

	public override void Initialize(Character to_follow){
		base.Initialize (to_follow);
		follow = to_follow;

		//Update ();
	}

	void Update(){
		if (follow == null) {
			Destroy (gameObject);
			return;
		}
		transform.position = follow.transform.position;
		
	}

	public void SetEnabled(bool to){
		foreach (SpriteRenderer s in transform.GetComponentsInChildren<SpriteRenderer>()) {
			s.enabled = to;
		}
	}

	protected override void Start ()
	{
		
	}
}
