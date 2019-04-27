using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
	
	public GameObject price_heart;

	public List<ShopButton> buttons = new List<ShopButton>();

	public bool active{
		get{
			return gameObject.activeSelf;
		}
		set{
			gameObject.SetActive (value);
		}
	}

	void Start(){
		active = false;
	}

	public void SetPrice(){
		buttons [0].SetPrice (4);
	}

	public void Close(){
		active = false;
	}
}
