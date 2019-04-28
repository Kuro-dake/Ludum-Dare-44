using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovement : Modifier{

	public override void Trigger ()
	{
		GM.player.free_movement_max += value;
		GM.player.free_movement = GM.player.free_movement_max;
	}
}
