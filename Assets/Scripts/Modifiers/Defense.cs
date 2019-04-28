using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : Modifier{

	public override string GetDescription (bool buy, int price)
	{
		return "Buy a shield that can block total of " + value + "\npoints of damage for " + price + " vitality. \n A shield blocks 2 out of 3 attacks.";
	}

	public override void Trigger ()
	{
		GM.player.defense += value;
	}
}
