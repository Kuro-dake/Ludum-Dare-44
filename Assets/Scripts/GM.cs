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

	void SetupValues(){
		Setup.ClearBuffer ();
		floor.GenerateGrid (Setup.base_settings.GetInt("grid_size"));
	}

	void Start(){
		SetupValues ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Z)) {
			SetupValues ();
		}
	}
}
