using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	public override void Initialize ()
	{
		apmax = 2;
		ap_regen = 2;

		hpbar = GameObject.Instantiate (GM.characters.monster_hpbar).GetComponent<MonsterHPBar>();
		hpbar.Initialize (this);
		hp = 3;
		base.Initialize ();
	}

	public bool Movement(){
		List<GridPosition> path = PathSearch.FindDirectPath (gp, GM.player.gp, new List<GridPosition>(new GridPosition[]{gp, GM.player.gp}));

		if (path.Count > 0) {
			GridPosition target = path [0];
			Move ((target.x - gp.x).Sign(), (target.y - gp.y).Sign());
			return true;
		}
		return false;

	}
}
