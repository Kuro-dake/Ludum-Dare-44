using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinema : MonoBehaviour {

	public GameObject genie;
	public GameObject peasant;
	public GameObject lord;

	public GameObject rtip;
	public GameObject ltip;

	public Transform fgscreen{
		get{
			return transform.Find ("FGScreen");
		}
	}

	public bool active{
		get{
			return gameObject.activeSelf;
		}
		set{
			gameObject.SetActive (value);
			GM.canvas.SetActive (!value);
		}
	}

	TextMesh btextmesh {
		get {
			return transform.Find("bubble").GetComponentInChildren<TextMesh> ();
		}
	}

	TextMesh fgstextmesh {
		get {
			return transform.Find("FGScreen").GetComponentInChildren<TextMesh> ();
		}
	}

	string bubble_text{
		set{
			btextmesh.text = value;
		}
	}

	Vector3 p_down{get{return new Vector3 (-25.3f, -40.1f, 1f);}}
	Vector3 p_talk{get{return new Vector3 (-25.3f, -18f, 1f);}}
	Vector3 p_gone{get{return new Vector3 (-40.9f, -18f, 1f);}}

	Vector3 g_down{get{return new Vector3 (.3f, 40.36f, 1f);}}
	Vector3 g_talk{get{return new Vector3 (.3f, -7.89f, 1f);}}
	Vector3 g_gone{get{return new Vector3 (19.3f, -7.89f, 1f);}}

	public int cinema_phase = 0;

	public void Initialize(){
		active = true;
		cinema_phase = 0;
		fgscreen.sr ().color = Color.clear;
		btextmesh.GetComponent<Renderer> ().sortingLayerName = "Cinema";
		btextmesh.GetComponent<Renderer> ().sortingOrder = 100;
		fgstextmesh.GetComponent<Renderer> ().sortingLayerName = "Cinema";
		fgstextmesh.GetComponent<Renderer> ().sortingOrder = 250;
		fgstextmesh.color = new Color(1f,1f,1f, 0f);
		rtip.SetActive (false);
		ltip.SetActive (false);
		MoveTo (peasant, p_down, true); 
		MoveTo (genie, g_down, true);
		PlayString (GM.dialogues.intros);

	}

	int current_line = 0;
	string[] lines;

	public void PlayString(string s){
		active = true;
		transform.parent.localScale = Vector3.one * GM.inst.cam_size_to_cinema_scale_and_genie_positions [GM.cam.ortosize][0]; // I'm going to hell for this as well
		MoveTo (lord, g_gone + Vector3.right * 25f, true);
		if (cinema_phase == 1) {
			MoveTo (peasant, p_gone, true); 
			MoveTo (genie, g_gone, true);

			MoveTo (peasant, p_talk);
			MoveTo (genie, g_talk);
		}

		lines = s.Split (new char[]{ '|' });
		current_line = 0;
		Progress ();
	}

	public bool Progress(){
		if (progress_routine != null) {
			return true;
		}
		if (current_line >= lines.Length ) {
			if (cinema_phase < 2) {
				active = false;
				return false;
			} else {
				if (cinema_phase == 2) {
					cinema_phase = 3;
					StartCoroutine (TheEnd ());
					return true;
				} else {
					return true;
				}
			}
		}

		progress_routine = StartCoroutine (ProgressStep (lines[current_line]));

		current_line++;
		return true;
	}

	IEnumerator FadeScreen(){
		while (fgscreen.sr ().color.a < 1f) {
			fgscreen.sr ().color += Color.black * (Time.deltaTime / 6f);
			yield return null;
		}
		yield return new WaitForSeconds (1f);
		StartCoroutine (FadeInText());
	}

	IEnumerator FadeInText(){
		while (fgstextmesh.color.a < 1f) {
			fgstextmesh.color += Color.black * (Time.deltaTime / 6f);
			yield return null;
		}
	}

	public IEnumerator TheEnd(){
		MoveTo (genie, g_gone, false, 3f);
		MoveTo (peasant, g_gone + Vector3.down * 8f, false, 4f);
		ltip.SetActive (false);
		rtip.SetActive (false);
		bubble_text = "o_O";
		yield return new WaitForSeconds(.5f);
		for (int i = 0; i < 30; i++) {
			GM.sounds.PlaySound (Random.Range (0, 2) == 1 ? 0 : 2);
			GM.cam.Shake ();
			if (i == 15) {
				StartCoroutine (FadeScreen ());
			}
			yield return new WaitForSeconds (Random.Range (.1f, .3f));
		}
		cinema_phase = 4;

	}

	public IEnumerator ProgressStep(string line){

		rtip.SetActive (false);
		ltip.SetActive (false);
		bubble_text = "";
		yield return null;
		string[] line_params = line.Split (new char[]{':'});
		string id = line_params [0].ToLower();
		string text = line_params [1];
		string[] actor_params = id.Split (new char[]{ '.' });
		string side = "";
		if(actor_params.Length > 1){
			side = actor_params [1];
			id = actor_params [0];
		}

		GameObject moved_char = null;
		Vector3 target = Vector3.one;
		switch (id) {
			case "n":
				bubble_text = text;
					break;
		case "h":
			moved_char = peasant;
			target = p_talk;
				bubble_text = text;
				ltip.SetActive (true);
				break;
		case "g":
			moved_char = genie;
			target = g_talk;
			bubble_text = text;
			rtip.SetActive (true);
			rtip.transform.localPosition = new Vector3 (.73f, 1.17f, 0f);
				break;
		case "l":
			moved_char = lord;
			target = g_talk + Vector3.left * 3f;
			bubble_text = text;
			rtip.SetActive (true);
			rtip.transform.localPosition = new Vector3 (.26f, 1.17f, 0f);
			break;
		case "ll":
			moved_char = lord;
			target = g_gone + Vector3.right * 5f;
			bubble_text = "...";
			rtip.SetActive (false);

			break;
		case "s":
			current_line = 100;
			break;
		default:
			throw new UnityException ("Unknown speaker " + id + " saying: " + text);
			break;
		}
		if (moved_char != null) {
			/*if (side != "") {
				Vector3 ls = moved_char.transform.localScale;
				string orientation = side.Substring (0, 1);
				ls.x = Mathf.Abs (ls.x) * (id == "g" ? -1 : 1) * (orientation == "l" ? 1 : -1);
				if (side == "ll") {
					MoveTo (moved_char, p_gone, true);
				}
				if (side == "rl") {
					MoveTo (moved_char, g_gone, true);
				}
				if (orientation == "r") {
					target = g_talk;
				}
				if (orientation == "l") {
					target = p_talk;
				}
				moved_char.transform.localScale = ls;
			}*/

			yield return MoveTo (moved_char, target);
		}


		progress_routine = null;
	}

	Coroutine progress_routine;

	Coroutine MoveTo(GameObject chr, Vector3 target, bool instant = false, float speed = 1f){
		if (instant) {
			chr.transform.position = target;
			return null;
		}
		return StartCoroutine (MoveToStep (chr, target, speed));

	}

	IEnumerator MoveToStep(GameObject chr, Vector3 target, float speed = 1f){
		Transform t = chr.transform;
		while (Vector3.Distance (t.localPosition, target) > 0f) {
			t.localPosition = Vector3.MoveTowards (t.localPosition, target, Time.deltaTime * 33f * speed);
			yield return null;
		}

	}
}
