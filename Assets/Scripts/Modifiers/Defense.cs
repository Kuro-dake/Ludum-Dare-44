using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : Modifier{

	public override void Trigger ()
	{
		GM.player.defense += value;
	}
}
