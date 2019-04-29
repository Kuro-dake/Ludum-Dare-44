using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genie : MonoBehaviour {
	[SerializeField]
	GameObject tail;
	List<GameObject> tail_fragments = new List<GameObject>();
	FloatRange tail_delay;
	public bool active = false;
	public bool active_tail = false;
	// Use this for initialization
	void Start () {
		tail_delay = new FloatRange(Setup.base_settings.AGetFloat("genie_tail_range"));
		bubble_text = "";
	}
	
	// Update is called once per frame
	float taildelay = 0f;
	float sound_delay = 0f;
	FloatRange pitch_range;
	void Update () {
		if (GM.cinema.active || GM.title.active) {
			return;
		}
		if (!active_tail) {
			GM.music.running = GM.player.is_alive ? 3 : 2;
			bubble_text = "";
			return;
		}

		GM.music.running = 2;
		pitch_range = new FloatRange (.5f, 1f);
		tail_fragments.RemoveAll (delegate(GameObject obj) {
			return obj == null;	
		});
		tail_fragments.ForEach (delegate(GameObject obj) {
			obj.sr().color -= Color.black * Time.deltaTime * Setup.base_settings.GetFloat("genie_tail_dissipate_speed");	
			obj.transform.localScale += Vector3.one * Time.deltaTime * Setup.base_settings.GetFloat("genie_tail_scale_rate");
			if(obj.sr().color.a <= 0f){
				Destroy(obj);
			}
		});

		if ((sound_delay -= Time.deltaTime) <= 0) {
			GM.sounds.PlaySound (Random.Range(3,5), pitch_range);
			GM.sounds.PlaySound (Random.Range(3,5), pitch_range);
			sound_delay = Random.Range (1.5f, 2.5f);
		}
		if ((taildelay -= Time.deltaTime) <= 0) {
			taildelay = tail_delay.random;
			GameObject tfragment = GameObject.Instantiate (tail);
			tfragment.transform.Rotate (Random.insideUnitSphere * 20f); 
			tfragment.transform.SetParent (transform, false);
			tfragment.transform.localPosition = Vector3.down * 2f;
			tfragment.sr ().flipX = Random.Range (0, 2) == 1;
			tfragment.transform.localScale = Vector3.zero;
			tail_fragments.Add (tfragment);
		}
	}
	float x_target{
		get{
			return GM.inst.cam_size_to_cinema_scale_and_genie_positions [GM.cam.ortosize][1]; // I'm going to hell for this as well
		}
	}
	int last_played = 0;
	IEnumerator GoTo(bool set_to){
		
		Vector3 target = new Vector3 (x_target, set_to ? -2.38f : 25f, 1f);
		Transform parent = transform.parent;
		if (set_to) {
			yield return new WaitForSeconds (.5f);
		}
		while (Vector3.Distance (parent.position, target) > 0f) {
			
			parent.position = Vector3.MoveTowards (parent.position, target, Time.deltaTime * Setup.base_settings.GetFloat ("genie_speed") * (set_to ? 1 : 3));
			yield return null;
		}
		active = set_to;
		active_tail = set_to;
		if (active) {
			if (GM.inst.current_level != last_played) {
				
				GM.cinema.PlayString (GM.dialogues.level_intros [GM.inst.current_level - 1]);
				last_played = GM.inst.current_level;
			}
		}
	}
	public void SetActive(bool set_to){
		if (set_to == active) {
			return;
		}
		if (set_to) {
			transform.parent.position = new Vector3 (x_target, -21.5f, 1f);
		} else {
			bubble_text = "";
		}
		GM.routines.CStart ("genie_move", GoTo (set_to));
		if (set_to) {
			active_tail = true;
		}

	}

	void OnMouseEnter(){
		if (GM.cinema.active) {
			return;
		}
		if (!GM.shop.has_any_wares) {
			bubble_text = "Start moving peasant!";
			return;
		}
		if (!GM.shop.active) {
			bubble_text = "Did you forget something, peasant?";
		}


	}

	void OnMouseExit(){
		bubble_text = "";
	}

	void OnMouseDown(){
		if (GM.cinema.active) {
			return;
		}
		if (!active) {
			return;
		}
		if (!GM.shop.has_any_wares) {
			bubble_text = "Quit your bullsh** and fight!";
			return;
		}
		bubble_text = "Let's do bussiness.";
		GM.shop.active = true;
		arrow_active = false;
		GM.sounds.Click ();
	}
	public bool arrow_active {
		set{
			transform.Find ("arrow").sr ().enabled = false;
		}
	}
	Coroutine broutine;
	IEnumerator ClearBubble(){
		yield return new WaitForSeconds (7f);
		Transform b = transform.parent.Find ("bubble");
		b.gameObject.SetActive (false);
		TextMesh m = b.GetComponentInChildren<TextMesh> ();
		m.text = "";
		broutine = null;
	}
	public void SetBubbleText(string value, bool traced_routine = true){
		Transform b = transform.parent.Find ("bubble");
		b.gameObject.SetActive (value.Length > 0);
		TextMesh m = b.GetComponentInChildren<TextMesh> ();
		m.GetComponent<Renderer> ().sortingLayerName = "UI";
		m.GetComponent<Renderer> ().sortingOrder = 13;
		m.text = value;
		if (broutine != null) {
			StopCoroutine (broutine);
		}
		broutine = StartCoroutine (ClearBubble ());
	}
	public string bubble_text{
		set{
			SetBubbleText (value);
		}
	}

}
