using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Pickup{

	bool taken = false;
	public int def = 0;
	public override void Take ()
	{
		if (taken || !active) {
			return;
		}
		taken = true;
		FadeOut ();
		GM.player.defense += def;
		GM.shield = true;
	}

}
