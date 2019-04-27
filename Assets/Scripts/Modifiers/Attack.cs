using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Modifier {

	public override void Trigger ()
	{
		GM.player.damage += value;
	}
}
