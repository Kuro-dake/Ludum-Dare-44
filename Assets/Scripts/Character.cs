using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	protected int x_pos, y_pos;

	Coroutine movement_routine = null;

	public virtual void Move(int x, int y){
		MoveTo (x_pos + x, y_pos + y);
	}

	public virtual void MoveTo(int x, int y){
		x_pos = Mathf.Clamp (x, 0, GM.floor.max_x);
		y_pos = Mathf.Clamp (y, 0, GM.floor.max_y);

		GM.routines.CStart ("char_move_" + this.GetInstanceID (), MovementRoutine(GM.floor [x_pos, y_pos]));

	}

	protected IEnumerator MovementRoutine(FloorTile tile){
		Vector3 target = tile.transform.position;
		while(Vector2.Distance(transform.position,target) > 0f){
			transform.position = Vector2.MoveTowards(transform.position, target , Time.deltaTime * Setup.base_settings.GetFloat("movement_speed"));
			yield return null;
		}

	}
}
