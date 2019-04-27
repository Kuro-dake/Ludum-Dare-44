using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	protected override HPBar hpbar {get {return GM.player_HPBar;}}

	public override int free_movement {
		get {
			return base.free_movement;
		}
		set {
			base.free_movement = value;
			GM.player_FMBar.DisplayHP (value);
		}
	}

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

	public override action_result MoveTo (int x, int y, bool instant = false)
	{
		return base.MoveTo (x, y, instant);
	}

	public override void Initialize ()
	{
		player_faction = true;
		base.Initialize ();
		hp = 4;
		free_movement_max = 0;
		ap = 1;
		MoveTo (0, 0, true);
	}

	public override void ReceiveDamage (int damage)
	{
		base.ReceiveDamage (damage);
		Debug.Log ("player hit");
	}
}
