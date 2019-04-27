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
}
