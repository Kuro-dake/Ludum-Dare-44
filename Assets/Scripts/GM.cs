using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour {

	public GameObject this[string s]{
		get{
			GameObject ret = GameObject.Find (s);
			if (ret == null) {
				throw new UnityException (s + " not found");
			}
			return ret;
		}
	}

	static GM _inst;
	public static GM inst{
		get{
			if (_inst == null) {
				_inst = GameObject.Find ("GM").GetComponent<GM> ();
			}
			return _inst;
		}
	}

	public static Floor floor{get{return inst ["Floor"].GetComponent<Floor>();}}
	public static Player player{ get { return inst ["Player"].GetComponent<Player> (); } }
	public static CoroutineContainer routines{ get { return inst ["CoroutineContainer"].GetComponent<CoroutineContainer> (); } }
	public static CharacterManager characters{ get { return inst ["Characters"].GetComponent<CharacterManager> (); } }
	public static HPBar player_HPBar{ get { return inst ["Player_HPBar"].GetComponent<HPBar> (); } }
	public static CamScript cam{ get { return inst ["Main Camera"].GetComponent<CamScript> (); } }

	void SetupValues(){
		Setup.ClearBuffer ();
		floor.GenerateGrid (Setup.base_settings.GetInt("grid_size"));
		characters.GenerateRandomEnemies ();
	}

	void Start(){
		SetupValues ();
		GM.player.Initialize ();

	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			SetupValues ();

		}
		if (Input.GetKeyDown (KeyCode.A)) {
			/*List<GridPosition> path = PathSearch.FindDirectPath (new GridPosition (0, 0), new GridPosition (4, 4));
			path.ForEach (delegate(GridPosition obj) {
				obj.ft.transform.sr().color = Color.blue;	
			});*/
		}
	}
}
