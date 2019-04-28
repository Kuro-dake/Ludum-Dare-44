using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Modifier {

	public override string GetDescription (bool buy, int price)
	{
		if(!buy)
			return base.GetDescription (buy, price);
		return "I can sell you a sword that'll increase \n the damage you deal by " + value + " points for " + price + " vitality";
	}

	public override string GetSpecDescription ()
	{
		return "damage per hit";
	}

	public override void Trigger ()
	{
		GM.player.damage += value;
	}
}
