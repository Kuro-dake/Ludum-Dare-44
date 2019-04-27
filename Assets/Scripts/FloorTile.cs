using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

	public GridPosition gp{ get;protected set; }

	public void Init(int x, int y){
		gp = new GridPosition (x, y);
	}

	public Character occupant{get {return GM.characters [gp.x, gp.y];}}
}
