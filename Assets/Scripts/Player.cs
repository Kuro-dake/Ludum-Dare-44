using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	protected override HPBar hpbar {get {return GM.player_HPBar;}}

	public override int ap {
		get {
			return base.ap;
		}
		set {
			base.ap = value;
			GM.player_APBar.DisplayHP (value);
		}
	}

	public override void Move (int x, int y)
	{
		base.Move (x, y);
		if (ap <= 0) {
			GM.inst.EnemyTurn ();
		}
	}

	public override void MoveTo (int x, int y, bool instant = false)
	{
		base.MoveTo (x, y, instant);
	}

	public override void Initialize ()
	{
		player_faction = true;
		base.Initialize ();
		hp = 4;
	}

	public override void ReceiveDamage (int damage)
	{
		base.ReceiveDamage (damage);
		Debug.Log ("player hit");
	}
}
