using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

	List<Character> _characters = new List<Character>();
	List<Character> characters{
		get{
			
			return _characters.FindAll(delegate(Character obj) {
				//Debug.Log(obj.name + " is " + (obj == null ? "" : " not ") + " null and "+ (obj.is_alive ? " is alive " : " is dead")); 
				return obj != null && obj.is_alive;
			});

		}
	}
	public List<Enemy> enemies {
		get{
			return _characters.FindAll (delegate(Character obj) {
				return obj is Enemy;	
			}).ConvertAll<Enemy> (delegate(Character input) {
				return input as Enemy;	
			});
		}
	}
	[SerializeField]
	public MonsterHPBar monster_hpbar;
	[SerializeField]
	Enemy[] enemy_types;

	public void GenerateRandomEnemies(){
		characters.ForEach (delegate(Character obj) {
			if(obj is Enemy){
				Destroy(obj.gameObject);
			}	
		});
		characters.RemoveAll(delegate(Character obj) {
			return obj is Enemy;
		});
		for (int i = 0; i < 1; i++) {
			GenerateRandomEnemy ();
		}
	}

	public void GenerateEnemy(int x, int y){
		
		Enemy en = GameObject.Instantiate (enemy_types[0].gameObject).GetComponent<Enemy>();
		en.MoveTo (x, y, true);
		en.transform.localScale *= Random.Range (.9f, 1.1f);
		en.GetComponent<Animator> ().speed = Random.Range (.9f, 1.1f);
		en.TrackCharacter ();
		en.Orientation (Random.Range (0, 2) == 1);
		Color enemy_color = Color.white * Random.Range (.9f, 1f);
		enemy_color.a = 1f;
		foreach (SpriteRenderer sr in en.transform.GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = enemy_color; 
		}
		en.sr ().color = Color.black;
		en.Initialize ();

	}

	public void GenerateRandomEnemy(){
		List<GridPosition> free = GM.floor.GetUnoccupiedFieldPositions ();
		free.Shuffle();
		GridPosition gp = free [0];
		GenerateEnemy (gp.x, gp.y);

	}

	public void TrackCharacter(Character ch){
		_characters.Add (ch);
	}

	public Character this[int x, int y]{
		get{
			return characters.Find (delegate(Character obj) {
				return obj.x_position == x && obj.y_position == y;	
			});
		}
	}

	IEnumerator EnemyTurnStep(){
		ClearDead ();
		while (GM.routines.any_routines_running) {
			yield return null;
		}
		foreach(Character c in characters){
			if(!(c is Enemy)){
				continue;
			}	

			if (c == null) {
				continue;
			}
			while (c.ap > 0) {
				Debug.Log (c.ap);
				if ((c as Enemy).Movement ()) {
					while (GM.routines.any_routines_running) {
						yield return null;
					}
					yield return new WaitForSeconds (.1f);
				} else {
					break;
				}
			}


		}
		while (GM.routines.any_routines_running) {
			yield return null;
		}
		GM.inst.StartTurn ();
		ClearDead ();
		if (enemies.Count == 0) {
			GM.inst.NextLevel ();
		}
	}
	protected void ClearDead(){
		_characters.RemoveAll (delegate(Character obj) {
			return obj == null || !obj.is_alive;
		});
	}
	public void EnemyTurn(){
		enemies.ForEach (delegate(Enemy obj) {
			obj.StartTurn();	
		});
		StartCoroutine(EnemyTurnStep ());
	}


}
