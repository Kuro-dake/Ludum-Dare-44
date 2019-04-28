using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounds : MonoBehaviour {
	public float volume = .6f;
	public bool _is_on = false;
	public bool is_on{
		get{
			return _is_on;
		}
		set {
			_is_on = value;
			sources.ForEach (delegate(AudioSource obj) {
				obj.volume = value ? volume : 0f;	
			});
			PlayerPrefs.SetInt ("sound", value ? 1 : 0);
			PlayerPrefs.Save ();
		}
	}
	[SerializeField]
	List<AudioClip> clips = new List<AudioClip>();

	List<AudioSource> sources = new List<AudioSource>();
	public void Click(){
		PlaySound (5, 1f, new FloatRange (.6f, .6f));
	}
	public void PlaySound(int clip_number, float pitch_multiplyer = 1f, FloatRange pitch_range = null){
		if (!is_on) {
			return;
		}
		if (pitch_range == null) {
			pitch_range = new FloatRange (.6f, 1f);
		}
		AudioSource use = sources.Find (delegate(AudioSource obj) {
			return !obj.isPlaying;
		});
		if (use == null) {
			use = sources [Random.Range(0, sources.Count)];
			use.Stop ();
		}
		use.clip = clips [clip_number];
		use.pitch = pitch_range.random * pitch_multiplyer;
		use.Play ();
	}


	public void RandomSound(){
		PlaySound (Random.Range (0, clips.Count));
	}
	[SerializeField]
	int sources_number = 5;

	void Start(){
		for (int i = 0; i < sources_number; i++) {
			AudioSource s = new GameObject ("source " + i ).AddComponent<AudioSource> ();
			s.transform.SetParent (transform);
			s.playOnAwake = false;
			s.Stop ();
			s.volume = volume;
			s.loop = false;
			sources.Add(s);
		}
	}
}
