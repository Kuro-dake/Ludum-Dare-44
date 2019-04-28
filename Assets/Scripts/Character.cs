using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {
	int _ap;
	int _free_movement;
	public virtual int free_movement{
		get{
			return _free_movement;
		}
		set {
			_free_movement = value;
		}
	}
	public int free_movement_max = 0;
	public int ap_regen = 1;
	public virtual int ap{
		get{
			return _ap;
		}
		set{
			_ap = value;
		}
	}
	public int apmax = 3;
	[SerializeField]
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
	[SerializeField]
	protected int _hp = 1;
	public int hp{ get{ return _hp;} 
		set{ 
			_hp = value;
			hpbar.DisplayHP (_hp);

		}}
	
	public int _damage = 1;
	public virtual int damage{
		get{
			return _damage;
		}
		set{
			_damage = value;
		}
	}
	[SerializeField]
	protected bool player_faction = false;

	public GridPosition gp {get {return new GridPosition (x_position, y_position);}}

	Coroutine movement_routine = null;

	public void Orientation(bool left){
		transform.localScale = new Vector3 (Mathf.Abs (transform.localScale.x) * (left  ? 1 : -1), transform.localScale.y, 1f);
	}

	public virtual action_result Move(int x, int y){
		action_result res = MoveTo (x_pos + x, y_pos + y);
		if (res == action_result.attack) {
			ap -= 1;
		} else if(res == action_result.movement) {
			if (free_movement > 0) {
				free_movement -= 1;
			} else {
				ap -= 1;
			}
		}

		if (x != 0) {
			Orientation (x > 0);
		}
		return res;
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

	public virtual action_result MoveTo(int x, int y, bool instant = false){ // interact with

		if (instant) {
			x_pos = x;
			y_pos = y;
			transform.position = GM.floor [x_pos, y_pos].transform.position;
			return action_result.movement;
		}

		x = Mathf.Clamp (x, 0, GM.floor.max_x);
		y = Mathf.Clamp (y, 0, GM.floor.max_y);

		if (x == gp.x && y == gp.y) {
			return action_result.none;
		}

		FloorTile ft = GM.floor [x, y];

		if (ft.occupant != null && ft.occupant != this && ft.occupant.player_faction != player_faction) {
			if (ap > 0) {
				GM.routines.CStart ("char_attack_" + GetInstanceID (), Attack (ft.occupant));
				return action_result.attack;
			}
			return action_result.none;
		}

		x_pos = x;
		y_pos = y;

		GM.routines.CStart ("char_move_" + this.GetInstanceID (), MovementRoutine (GM.floor [x_pos, y_pos]));
		return action_result.movement;



	}

	public IEnumerator MovementRoutine(FloorTile tile){
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
			GM.routines.CStart("die_" + GetInstanceID(), FadeDie ());
		}
		GM.cam.Shake ();
	}
	protected void FadeIn(){
		GM.routines.CStart ("fadein_" + GetInstanceID() ,FadeInStep ());
	}
	IEnumerator FadeInStep(){
		yield return new WaitForSeconds (Random.Range (.3f, .6f));
		Color c = transform.sr ().color;
		c.a = 0f;
		transform.sr ().color = c;
		while (transform.sr ().color.a < 1f) {
			//transform.sr().color += Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>()) {
				
				sr.color += Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			}
			yield return null;

		}
		if (!(this is Player)) {
			(hpbar as MonsterHPBar).SetEnabled(true);
			hpbar.Initialize (this);
			hp = hp;
		}
	}

	IEnumerator FadeDie(){
		if (this is Player) {
			GM.sword = false;
			GM.shield = false;
		}
		while (transform.sr ().color.a > 0f) {
			//transform.sr().color -= Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>()) {
				sr.color -= Color.black * Time.deltaTime * Setup.base_settings.GetFloat ("dying_fade_speed");
			}
			yield return null;
		}
		if (!(this is Player)) {
			Destroy (gameObject);
		} else {
			gameObject.sr ().color = new Color (1f, 1f, 1f, 0f);
		}

	}

	/*protected virtual void Start(){
		

	}*/

	public virtual void Initialize(){
		foreach (Transform t in transform.GetComponentsInChildren<Transform>()) {
			SpriteOrder so = null;
			if (t.gameObject.GetComponent<SpriteOrder> () == null) {
				so = t.gameObject.AddComponent<SpriteOrder> ();
			}
			if (so == null) {
				continue;
			}
			so.Init (transform);
		}
		foreach (SpriteRenderer s in transform.GetComponentsInChildren<SpriteRenderer>()) {
			if(new List<string>(new string[]{"Sword", "Shield", "shand"}).Contains(s.gameObject.name )){
				continue;
			}
			Color c = s.color;
			c.a = 0f;
			s.color = c;
		}
		if (!(this is Player)) {
			(hpbar as MonsterHPBar).SetEnabled(false);

		}
		FadeIn ();
		TrackCharacter ();
		MoveTo (x_pos, y_pos, true);
		ap = apmax;
		first_turn = true;
		hp = hp;
		free_movement = free_movement_max;
	}
	bool first_turn = true;
	public virtual void StartTurn(){
		free_movement = free_movement_max;

		if (!first_turn) {
			free_movement += ap;
		}
		first_turn = false;
		ap = apmax;//Mathf.Clamp(Mathf.Clamp(ap, 0, apmax) + ap_regen, 0, apmax);

	}
}

public enum action_result{
	movement,
	attack,
	none
}