using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shop : MonoBehaviour {
	
	public GameObject price_heart;

	public List<ShopButton> buttons = new List<ShopButton>();

	public bool active{
		get{
			return gameObject.activeSelf;
		}
		set{
			Debug.Log ("setting gameshop " + value);
			gameObject.SetActive (value);
		}
	}

	void Start(){
		//active = false;
	}

	public void SetPrice(){
		buttons [0].SetPrice (4);
	}

	public void Close(){
		active = false;
	}

	public void SetButtons(int[] btns, int[] prices, int[] values){
		buttons.ForEach (delegate(ShopButton obj) {
			obj.GetComponent<Button>().interactable = false;
			obj.SetPrice(0);
		});
		for (int i = 0; i < btns.Length; i++) {
			buttons [btns[i]].GetComponent<Button> ().interactable = true;
			buttons [btns [i]].SetPrice (prices [i]);
			buttons [btns [i]].SetValue(values [i]);
		}
	}
}
