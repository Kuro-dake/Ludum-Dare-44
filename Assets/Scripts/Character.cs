using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	protected int x_pos, y_pos;
	bool tracking = false;
	public int x_position{get{return x_pos;}}
	public int y_position{get{return y_pos;}}
	HPBar _hpbar;
	protected virtual HPBar hpbar {
		get{return _hpbar;}
		set{ _hpbar = value; }
	}

	public bool is_alive{
		get{
			return hp > 0;
		}
	}

	protected int _hp = 1;
	public int hp{ get{ return _hp;} 
		protected set{ 
			_hp = value;
			hpbar.DisplayHP (_hp);

		}}
	public int damage = 1;

	protected bool player_faction = false;

	public GridPosition gp {get {return new GridPosition (x_position, y_position);}}

	Coroutine movement_routine = null;

	public void Orientation(bool left){
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * (left  ? 1 : -1), transform.localScale.y, 1f);
	}

	public virtual void Move(int x, int y){
		MoveTo (x_pos + x, y_pos + y);
		if (x != 0) {
			Orientation (x > 0);
		}
	}

	protected virtual IEnumerator Attack(Character target){
		while (Vector2.Distance (transform.position, target.transform.position) > Setup.base_settings.GetFloat("attack_distance")) {
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, Time.deltaTime * Setup.base_settings.GetFloat ("attack_speed"));
			yield return null;
		}
		target.ReceiveDamage (damage);

		FloorTile ft = GM.floor [x_pos, y_pos];
		while (Vector2.Distance (transform.position, ft.transform.position) > 0f) {
			transform.position = Vector3.MoveTowards (transform.position, ft.transform.position, Time.deltaTime * Setup.base_settings.GetFloat ("attack_speed"));
			yield return null;
		}

	}

	public virtual void MoveTo(int x, int y, bool instant = false){ // interact with

		x = Mathf.Clamp (x, 0, GM.floor.max_x);
		y = Mathf.Clamp (y, 0, GM.floor.max_y);

		FloorTile ft = GM.floor [x, y];

		if (ft.occupant != null && ft.occupant != this && ft.occupant.player_faction != player_faction) {
			GM.routines.CStart ("char_attack_" + GetInstanceID() ,Attack (ft.occupant));
			return;
		}

		x_pos = x;
		y_pos = y;

		if (instant) {
			transform.position = GM.floor [x_pos, y_pos].transform.position;
		}
		else {
			GM.routines.CStart ("char_move_" + this.GetInstanceID (), MovementRoutine (GM.floor [x_pos, y_pos]));
		}


	}

	protected IEnumerator MovementRoutine(FloorTile tile){
		Vector3 target = tile.transform.position;
		while(Vector2.Distance(transform.position,target) > 0f){
			transform.position = Vector2.MoveTowards(transform.position, target , Time.deltaTime * Setup.base_settings.GetFloat("movement_speed"));
			yield return null;
		}

	}

	public void TrackCharacter(){
		if (!tracking) {
			GM.characters.TrackCharacter (this);
			tracking = true;
		}
	}

	public virtual void ReceiveDamage(int damage){
		//hp = Random.Range (3, 6);
		hp -= damage;
		if (hp <= 0) {
			StartCoroutine (FadeDie ());
		}
		GM.cam.Shake ();
	}

	IEnumerator FadeDie(){
		while (transform.sr ().color.a > 0f) {
			transform.sr().color -= Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>()) {
				sr.color -= Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			}
			yield return null;
		}
		Destroy (gameObject);
	}

	void Start(){
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			SpriteOrder so = t.gameObject.AddComponent<SpriteOrder> ();
			if (so == null) {
				continue;
			}
			so.Init (transform);
		}
		TrackCharacter ();
	}

	public virtual void Initialize(){
		MoveTo (x_pos, y_pos, true);
	}
}
