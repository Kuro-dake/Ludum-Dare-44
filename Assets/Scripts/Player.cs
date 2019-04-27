using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	protected override HPBar hpbar {get {return GM.player_HPBar;}}

	public override void MoveTo (int x, int y, bool instant = false)
	{
		base.MoveTo (x, y, instant);
	}

	public override void Initialize ()
	{
		player_faction = true;
		base.Initialize ();
		hp = 3;
	}

	public override void ReceiveDamage (int damage)
	{
		base.ReceiveDamage (damage);
		Debug.Log ("player hit");
	}
}
