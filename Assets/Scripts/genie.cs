using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class genie : MonoBehaviour {
	[SerializeField]
	GameObject tail;
	List<GameObject> tail_fragments = new List<GameObject>();
	FloatRange tail_delay;
	// Use this for initialization
	void Start () {
		tail_delay = new FloatRange(Setup.base_settings.AGetFloat("genie_tail_range"));
	}
	
	// Update is called once per frame
	float taildelay = 0f;
	void Update () {
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

	void OnMouseDown(){
		Destroy (gameObject);
	}

}
