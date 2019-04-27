using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	public override void Initialize ()
	{
		
		base.Initialize ();
		hpbar = GameObject.Instantiate (GM.characters.monster_hpbar).GetComponent<MonsterHPBar>();
		hpbar.Initialize (this);
		hp = 4;
	}
}
