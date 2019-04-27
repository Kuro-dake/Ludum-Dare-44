using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineContainer : MonoBehaviour {

	Dictionary<string, Coroutine> routines = new Dictionary<string, Coroutine>();
	public List<string> running_routines = new List<string> ();
	public IEnumerator RunRoutine(string routine_name, Coroutine routine){
		yield return routine;
		Stop (routine_name);
	}

	void devrefreshpublic(){
		running_routines.Clear ();
		foreach (KeyValuePair<string, Coroutine> d in routines) {
			running_routines.Add (d.Key);
		}
	}

	public Coroutine CStart(string routine_name, IEnumerator routine_enum){
		Stop (routine_name);
		Coroutine routine = StartCoroutine (routine_enum);
		routines.Add (routine_name, routine);
		StartCoroutine (RunRoutine(routine_name, routine));
		devrefreshpublic ();
		return routine;
	}

	public bool IsRunning(string routine_name){
		return routines.ContainsKey (routine_name);
	}

	public void Stop(string routine_name){
		if (routines.ContainsKey (routine_name) && routines [routine_name] != null) {
			StopCoroutine (routines [routine_name]);
		}
		if(routines.ContainsKey(routine_name)){
			routines.Remove (routine_name);
		}
		devrefreshpublic ();
	}

}
