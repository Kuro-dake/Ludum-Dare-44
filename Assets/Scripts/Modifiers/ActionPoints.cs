using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPoints : Modifier {

	public override void Trigger ()
	{
		GM.player.apmax += value;
	}
}
