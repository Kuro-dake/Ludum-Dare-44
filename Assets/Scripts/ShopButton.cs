﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {
	[SerializeField]
	int price = 3;
	[SerializeField]
	bool buy = true;

	void Start(){
		SetPrice (price);
		ShowToggled ();
	}
	public void SetPrice(int set_price){
		Transform price_bar = transform.Find ("price");
		price_bar.DestroyChildren ();
		price = set_price;
		float margin = 15f;
		float offset = (-margin * (price - 1)) /2f;
		for (int i = 0; i < price; i++) {
			
			GameObject heart = Instantiate (GM.shop.price_heart);
			heart.transform.SetParent (price_bar, true);
			heart.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * (i * margin + offset);
			if (buy) {
				heart.GetComponent<Image> ().color = new Color (.2f, .2f, .2f, 1f);
			}
		}
	}

	public void Click(){
		
		Modifier mod = gameObject.GetComponent<Modifier> ();
		int change = price * (!mod.active ? 1 : -1) * (buy ? -1 : 1);
		if (GM.player.hp + change > 0) {
			GM.player.hp += change;
			mod.Toggle ();
			ShowToggled ();
		} else {
			// TODO show no hp left
			Debug.Log("not allowed");
		}
	}

	void ShowToggled(){

		Modifier mod = gameObject.GetComponent<Modifier> ();

		GetComponent<Image> ().color = mod.active ? Color.white : new Color (.6f, .6f, .6f, 1f);
		transform.Find ("Image").GetComponent<RectTransform> ().localScale = mod.active ? Vector3.one : Vector3.one * .7f;

	}

	public bool active{
		get{
			return gameObject.activeSelf;
		}
		set{
			gameObject.SetActive (value);
		}
	}
}
