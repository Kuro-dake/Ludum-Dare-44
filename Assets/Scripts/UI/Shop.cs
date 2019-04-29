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

	public void SetPrice(){
		buttons [0].SetPrice (4);
	}

	public void Close(){
		active = false;
		GM.routines.Stop ("genie_speech");
		GM.genie.SetBubbleText("Make me proud peasant.", false);
	}

	public void SetButtons(int[] btns, int[] prices, int[] values){
		buttons.ForEach (delegate(ShopButton obj) {
			obj.GetComponent<Button>().interactable = false;
			obj.transform.Find("Image").gameObject.SetActive(false);
			obj.SetPrice(0);
		});
		for (int i = 0; i < btns.Length; i++) {
			buttons [btns[i]].GetComponent<Button> ().interactable = true;
			buttons [btns [i]].SetPrice (prices [i]);
			buttons [btns [i]].SetValue(values [i]);
			buttons [btns [i]].transform.Find("Image").gameObject.SetActive(true);
		}
	}

	public void UnclickButtons(){
		buttons.ForEach (delegate(ShopButton obj) {
			obj.mod.active = false;
			obj.ShowToggled();
		});
	}

	public bool has_any_wares {
		get{
			return null != buttons.Find (delegate(ShopButton obj) {
				return obj.GetComponent<Button>().interactable;	
			});
		}
	}
}
