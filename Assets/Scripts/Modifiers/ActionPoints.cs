using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoints : Modifier {

	public override string GetSpecDescription ()
	{
		return "actions per round";
	}

	public override void Trigger ()
	{
		GM.player.apmax += value;
		GM.player.ap = GM.player.apmax;
	}
}
