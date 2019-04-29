using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer : MonoBehaviour {
	[TextArea(5,10)]
	public string intros;

	[TextArea(5,10)]
	public List<string> intro = new List<string> ();

	[TextArea(10,15)]
	public List<string> level_intros = new List<string>();

	void Start(){
		LoadLevelIntros ();
	}

	void LoadLevelIntros(){
		string text = Resources.Load<TextAsset> ("dialogues").text;
		level_intros = new List<string>(text.Split (new string[]{ "\n--\n" }, System.StringSplitOptions.RemoveEmptyEntries));
	}
}
