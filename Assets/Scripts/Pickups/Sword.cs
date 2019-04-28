using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Pickup {
	bool taken = false;
	public int damage = 0;
	public override void Take ()
	{
		if (taken || !active) {
			return;
		}
		taken = true;
		FadeOut ();
		GM.player.damage += damage;
		GM.sword = true;
	}

}
