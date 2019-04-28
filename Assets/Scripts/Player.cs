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

	public int _defense = 0;
	public int defense {
		get{
			return _defense;
		}
		set{
			_defense = value;
			GM.player_DEFBar.DisplayHP (value);
		}
	}
	protected override IEnumerator Attack (Character target)
	{
		yield return base.Attack (target);
		if (GM.characters.living_enemies.Count == 0) {
			GM.inst.NextLevel ();
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

	public override action_result Move (int x, int y)
	{
		action_result res = base.Move (x, y);
		if (ap <= 0 && free_movement <= 0) {
			EndTurn ();
		}
		return res;
	}
	bool received_border_damage = false;
	public override action_result MoveTo (int x, int y, bool instant = false)
	{
		action_result ret = base.MoveTo (x, y, instant);
		if (ret == action_result.movement) {
			ReceiveBorderDamage ();
			if (GM.inst.pickup_sword != null && gp == GM.inst.pickup_sword.gp) {
				GM.inst.pickup_sword.Take ();
			}
			if (GM.inst.pickup_shield != null && gp == GM.inst.pickup_shield.gp) {
				GM.inst.pickup_shield.Take ();
			}
		}
		return ret;

	}
	public override int damage {
		get {
			return base.damage;
		}
		set {
			base.damage = value;
			GM.player_ATTBar.DisplayHP (value);
		}
	}
	public override void Initialize ()
	{
		
		player_faction = true;

		ap = apmax;


		defense = 0;
		if (GM.floor.border_damage > 0) {
			x_pos = 1;
			y_pos = 1;
		}
		else {
			x_pos = 0;
			y_pos = 0;
		}
		base.Initialize ();
		GM.sword = false;
		GM.shield = false;

	}

	/*protected override void Start ()
	{
		base.Start ();
		GM.sword = false;
		GM.shield = false;
	}*/
	int last_hit = 0;

	public void ReceiveBorderDamage(){
		if (GM.floor.IsBorderTile (gp) && GM.floor.border_damage > 0 && !received_border_damage) {
			ReceiveDamage (GM.floor.border_damage);
			received_border_damage = true;
			GM.cam.Shake ();
		}
	}

	public override void ReceiveDamage (int damage)
	{
		if (defense > 0) {
			if (last_hit < 2) {
				defense -= damage;
				if (defense <= 0) {
					GM.shield = false;
				}
			} else {
				last_hit = -1;
				base.ReceiveDamage (damage);
			}
			last_hit++;
		} else {
			base.ReceiveDamage (damage);
		}

	}

	void Update(){
		GetComponent<Animator> ().SetBool ("armed", GM.sword || GM.shield);
	}

	public override void StartTurn ()
	{
		base.StartTurn ();
		received_border_damage = false;
	}

	public void EndTurn(){
		GM.inst.EnemyTurn ();
	}
}
