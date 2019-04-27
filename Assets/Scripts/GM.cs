using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using YamlDotNet.RepresentationModel;

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
	public static HPBar player_APBar{ get { return inst ["Player_APBar"].GetComponent<HPBar> (); } }

	public static CamScript cam{ get { return inst ["Main Camera"].GetComponent<CamScript> (); } }

	int current_level = 1;

	void LoadLevel(int level){
		YamlMappingNode level_setup = Setup.GetFile ("level" + level.ToString ());
		floor.GenerateGrid (level_setup.GetInt("x"), level_setup.GetInt("y"));

		foreach (YamlMappingNode mn in level_setup.GetNode<YamlSequenceNode>("enemies")) {
			characters.GenerateEnemy (mn.GetIntArray ("pos") [0], mn.GetIntArray ("pos") [1]);
		}

		GM.player.Initialize ();
		StartTurn ();
	}

	public void NextLevel(){
		current_level++;
		LoadLevel (current_level);
	}

	void SetupValues(){
		Setup.ClearBuffer ();
		LoadLevel (1);
	}

	void Start(){
		SetupValues ();
	}

	bool pathvis = false;
	List<GridPosition> lastpath;
	List<GameObject> lastrats;
	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			SetupValues ();
			resetcolor ();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			EnemyTurn ();
			//StartCoroutine (Walkpath (PathSearch.FindDirectPath (player.gp, new GridPosition (floor.max_x, floor.max_y), new List<GridPosition>(new GridPosition[]{player.gp}))));
			return;
			if (!pathvis) {
				
				List<GridPosition> path = PathSearch.FindDirectPath (player.gp, new GridPosition (floor.max_x, floor.max_y));
				path.ForEach (delegate(GridPosition obj) {
					obj.ft.transform.sr ().color = Color.blue;	
				});
				Debug.Log ("length " + path.Count);
				lastpath = path;
				lastrats = PathSearch.ratings;
			} else {
				
			}


		}
	}
	IEnumerator Walkpath(List<GridPosition> path){
		Queue<GridPosition> qp = new Queue<GridPosition> (path);
		while (qp.Count > 0) {
			yield return StartCoroutine (GM.player.MovementRoutine (qp.Dequeue ().ft));
			yield return new WaitForSeconds (.2f);
		}
	}
	void resetcolor(){
		

		PathSearch.ratings.ForEach (delegate(GameObject obj) {
			Destroy(obj);	
		});
	}
	public void StartTurn(){
		player.StartTurn ();

	}
	public void EnemyTurn(){
		GM.characters.EnemyTurn ();
	}
}
