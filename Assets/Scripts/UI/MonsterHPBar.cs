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

		for (int i = 0; i < hp; i++) {
			GameObject h = GameObject.Instantiate (heart);
			h.transform.SetParent (transform, false);
			h.transform.localPosition += Vector3.up * 1.64f * i;
		}
	}

	public override void Initialize(Character to_follow){
		follow = to_follow;
		base.Initialize (to_follow);
	}

	void Update(){
		if (follow == null) {
			Destroy (gameObject);
			return;
		}
		transform.position = follow.transform.position;
		
	}
	protected override void Start ()
	{
		
	}
}
