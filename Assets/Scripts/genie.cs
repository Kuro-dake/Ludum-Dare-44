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
	}
	
	// Update is called once per frame
	float taildelay = 0f;
	void Update () {
		if (!active_tail) {
			return;
		}
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
	IEnumerator GoTo(bool set_to){
		Vector3 target = new Vector3 (7.33f, set_to ? 0.38f : 25f, 1f);
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
	}
	public void SetActive(bool set_to){
		if (set_to == active) {
			return;
		}
		if (set_to) {
			transform.parent.position = new Vector3 (7.33f, -21.5f, 1f);
		}
		GM.routines.CStart ("genie_move", GoTo (set_to));
		if (set_to) {
			active_tail = true;
		}

	}
	void OnMouseDown(){
		if (!active) {
			return;
		}
		GM.shop.active = true;
		Debug.Log ("clicked");
	}

}
