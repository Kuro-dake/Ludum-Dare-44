using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
	public int start_level = 1;
	static GM _inst;
	public static GM inst{
		get{
			if (_inst == null) {
				_inst = GameObject.Find ("GM").GetComponent<GM> ();
			}
			return _inst;
		}
	}

	public Sword pickup_sword;
	public Shield pickup_shield;

	public Sword pickup_sword_template;
	public Shield pickup_shield_template;

	public static Floor floor{get{return inst ["Floor"].GetComponent<Floor>();}}
	public static Player player{ get { return inst ["Player"].GetComponent<Player> (); } }
	public static CoroutineContainer routines{ get { return inst ["CoroutineContainer"].GetComponent<CoroutineContainer> (); } }
	public static CharacterManager characters{ get { return inst ["Characters"].GetComponent<CharacterManager> (); } }
	public static HPBar player_HPBar{ get { return inst ["Player_HPBar"].GetComponent<HPBar> (); } }
	public static HPBar player_APBar{ get { return inst ["Player_APBar"].GetComponent<HPBar> (); } }
	public static HPBar player_FMBar{ get { return inst ["Player_Freemove"].GetComponent<HPBar> (); } }
	public static HPBar player_DEFBar{ get { return inst ["Player_Defense"].GetComponent<HPBar> (); } }
	public static HPBar player_ATTBar{ get { return inst ["Player_ATTBar"].GetComponent<HPBar> (); } }
	public static sounds sounds{ get { return inst ["Sounds"].GetComponent<sounds> (); } }
	public static music music{ get { return inst ["Music"].GetComponent<music> (); } }
	public GameObject _cinema;
	public static Cinema cinema { get { return inst._cinema.GetComponent<Cinema> (); } }
	public GameObject _title;
	public static TitleScreen title{ get { return inst._title.GetComponent<TitleScreen> (); } }
	public GameObject _canvas;
	public static GameObject canvas { get { return inst._canvas; } }
	public static DialogueContainer dialogues { get { return inst ["Dialogues"].GetComponent<DialogueContainer> (); } }
		



	public Shop _shop;
	public static Shop shop{ get { return inst._shop;} }
	public static Genie genie{ get { return inst ["genie"].GetComponent<Genie> (); } }
	[SerializeField]
	GameObject sword_go, shield_go;

	public static bool sword{ get { return inst.sword_go.activeSelf; } set{ inst.sword_go.SetActive (value); } }
	public static bool shield{ get { return inst.shield_go.activeSelf; } set{ inst.shield_go.SetActive (value); } }


	public static CamScript cam{ get { return inst ["Main Camera"].GetComponent<CamScript> (); } }

	public int current_level = 1;

	void LoadLevel(int level){
		canvas.SetActive (true);
		characters.DestroyEnemies ();
		GM.shop.UnclickButtons ();
		current_level = level;
		YamlMappingNode level_setup = Setup.GetFile ("level" + level.ToString ());
		int border_damage = 0;
		try{
			border_damage = level_setup.GetInt("border_damage");
		}
		catch(KeyNotFoundException e){
		}
		floor.GenerateGrid (level_setup.GetInt("x"), level_setup.GetInt("y"), border_damage);
		GM.cam.GetComponent<Camera> ().orthographicSize = level_setup.GetInt ("cam");
		foreach (YamlMappingNode mn in level_setup.GetNode<YamlSequenceNode>("enemies")) {
			characters.GenerateEnemy (mn.GetIntArray ("pos") [0], mn.GetIntArray ("pos") [1], mn.GetInt("type"));
		}

		GM.player.Initialize ();
		shop.active = false;
		player.hp = level_setup.GetInt ("hp");
		player.apmax = level_setup.GetInt ("ap");
		player.free_movement_max = level_setup.GetInt ("free_movement");
		player.free_movement = level_setup.GetInt ("free_movement");
		player.damage = level_setup.GetInt ("damage");


		StartTurn ();
		genie.SetActive (true);
		try{
			shop.SetButtons (level_setup.GetIntArray ("items"), level_setup.GetIntArray ("prices"), level_setup.GetIntArray("values"));
			genie.arrow_active = true;
		}
		catch(KeyNotFoundException e){
			shop.SetButtons(new int[]{}, new int[]{}, new int[]{});
			genie.arrow_active = false;
		}
		if (pickup_sword != null) {
			Destroy (pickup_sword.gameObject);
		}
		if (pickup_shield != null) {
			Destroy (pickup_shield.gameObject);
		}
		try{
			GridPosition sgp = new GridPosition(level_setup.GetIntArray("sword"));
			int sdmg = level_setup.GetInt("sword_damage");
			GameObject s = Instantiate(pickup_sword_template.gameObject);
			pickup_sword = s.GetComponent<Sword>();
			pickup_sword.damage = sdmg;
			pickup_sword.Place(sgp);
		}
		catch(KeyNotFoundException e){
		}

		try{
			GridPosition sgp = new GridPosition(level_setup.GetIntArray("shield"));
			int sdmg = level_setup.GetInt("shield_defense");
			GameObject s = Instantiate(pickup_shield_template.gameObject);
			pickup_shield = s.GetComponent<Shield>();
			pickup_shield.def = sdmg;
			pickup_shield.Place(sgp);
		}
		catch(KeyNotFoundException e){
		}
		sword = false;
		shield = false;
		player.Initialize ();

		player.Orientation (true);
		player.StartTurn ();


	}

	public void RestartLevel(){
		LoadLevel (current_level);
	}

	public void DevStartLevel(){
		LoadLevel (start_level);
	}

	public void NextLevel(){
		current_level++;
		if (current_level < 9) {
			LoadLevel (current_level);
		} else {
			GM.cinema.cinema_phase = 2;
			GM.cinema.PlayString (GM.dialogues.level_intros [8]);
			player.FadeDieNow ();
		}
	}

	void SetupValues(){
		Setup.ClearBuffer ();
		LoadLevel (start_level);
	}

	void Start(){
		title.Initialize ();
		//cinema.Initialize ();
		//SetupValues ();
	}

	bool pathvis = false;
	List<GridPosition> lastpath;
	List<GameObject> lastrats;
	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			return;
			SetupValues ();
			resetcolor ();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			return;

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
	public bool player_turn = true;
	public void StartTurn(){
		player_turn = true;
		player.StartTurn ();

	}
	public void EnemyTurn(){
		player_turn = false;
		GM.player.ReceiveBorderDamage();
		GM.characters.EnemyTurn ();
	}

	public Dictionary<int, float[]> cam_size_to_cinema_scale_and_genie_positions = new Dictionary<int, float[]>(){ // I'm going to hell for this
		{ 10, new float[]{.75f, 7.33f} } ,
		{ 11, new float[]{.85f, 8.33f} } ,
		{ 13, new float[]{1f, 15.29f} } ,
	};
}
