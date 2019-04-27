using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour {
	[SerializeField]
	protected int value;
	public bool active = false;
	public abstract void Trigger ();
	public void Toggle(){
		Trigger ();
		active = !active;
		value *= -1;
	}

}
