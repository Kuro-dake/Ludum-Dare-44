using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

	List<Character> _characters = new List<Character>();
	List<Character> characters{
		get{
			_characters.RemoveAll (delegate(Character obj) {
				return obj == null || !obj.is_alive;
			});
			return _characters;
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
		for (int i = 0; i < 4; i++) {
			GenerateRandomEnemy ();
		}
	}

	public void GenerateRandomEnemy(){
		List<GridPosition> free = GM.floor.GetUnoccupiedFieldPositions ();
		free.Shuffle();
		GridPosition gp = free [0];
		Enemy en = GameObject.Instantiate (enemy_types[0].gameObject).GetComponent<Enemy>();
		en.MoveTo (gp.x, gp.y, true);
		en.transform.localScale *= Random.Range (.9f, 1.1f);
		en.GetComponent<Animator> ().speed = Random.Range (.9f, 1.1f);
		en.TrackCharacter ();
		en.Orientation (Random.Range (0, 2) == 1);
		Color enemy_color = Color.white * Random.Range (.9f, 1f);
		enemy_color.a = 1f;
		foreach (SpriteRenderer sr in en.transform.GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = enemy_color; 
		}
		en.Initialize ();

	}

	public void TrackCharacter(Character ch){
		characters.Add (ch);
	}

	public Character this[int x, int y]{
		get{
			return characters.Find (delegate(Character obj) {
				return obj.x_position == x && obj.y_position == y;	
			});
		}
	}


}
