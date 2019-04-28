using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour {

	int hierarchical_order;
	Transform reference;
	public void Init(Transform refer){
		if (transform.sr () == null) {
			Destroy (this);return;
		}

		hierarchical_order = transform.sr ().sortingOrder;
		reference = refer;
	}

	public void Update(){
		if (transform.sr () == null) {
			return;
		}
		transform.sr ().sortingOrder = hierarchical_order + Mathf.RoundToInt(-reference.position.y * 40);
	}

}
