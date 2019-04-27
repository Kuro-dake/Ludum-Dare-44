using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrder : MonoBehaviour {

	int hierarchical_order;
	Transform reference;
	public void Init(Transform refer){
		hierarchical_order = transform.sr ().sortingOrder;
		reference = refer;
	}

	public void Update(){
		transform.sr ().sortingOrder = hierarchical_order + Mathf.RoundToInt(-reference.position.y * 40);
	}

}
