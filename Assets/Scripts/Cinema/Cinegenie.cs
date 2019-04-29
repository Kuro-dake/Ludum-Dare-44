using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinegenie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		tail_delay = new FloatRange(Setup.base_settings.AGetFloat("genie_tail_range"));
	}

	[SerializeField]
	GameObject tail;
	List<GameObject> tail_fragments = new List<GameObject>();
	FloatRange tail_delay;

	float taildelay = 0f;
	float sound_delay = 0f;
	FloatRange pitch_range;

	// Update is called once per frame
	void Update () {

		GM.music.running = GM.cinema.cinema_phase > 1 ? 0 : 2;

		if (GM.cinema.cinema_phase == 3) {
			return;
		}

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
			tfragment.transform.localPosition = Vector3.down * 2.36f;
			tfragment.sr ().flipX = Random.Range (0, 2) == 1;
			tfragment.transform.localScale = Vector3.zero;
			tail_fragments.Add (tfragment);
			tfragment.sr ().sortingLayerName = "Cinema";
			tfragment.sr ().sortingOrder = lasttailfront ? 27 : 29;
			lasttailfront = !lasttailfront;
		}
	}
	bool lasttailfront = false;
}
