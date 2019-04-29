using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
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
		float margin = Mathf.Clamp(50f / set_price, 0f, 15f);
		float offset = (-margin * (price - 1)) /2f;
		for (int i = 0; i < price; i++) {
			
			GameObject heart = Instantiate (GM.shop.price_heart);
			heart.transform.SetParent (price_bar, true);
			heart.GetComponent<RectTransform> ().anchoredPosition = Vector3.right * (i * margin + offset);
			heart.GetComponent<Image> ().color = buy ? new Color (0f, .3f, .6f, 1f) : Color.red;

		}
	}
	public Modifier mod {get{return gameObject.GetComponent<Modifier> ();}}
	public virtual void Click(){
		

		int change = price * (!mod.active ? 1 : -1) * (buy ? -1 : 1);
		if (GM.player.hp + change > 0) {
			GM.player.hp += change;
			mod.Toggle ();

			ShowToggled ();

			GM.genie.bubble_text = mod.active ? "Pleasure doing business with you, peasant." : "Okay, I'm giving it back to you.";

		} else {
			// TODO show no hp left
			Debug.Log("not allowed");
		}

	}



	public void ShowToggled(){

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

	public void SetValue(int value){
		mod.value = value;
	}

	public void OnPointerEnter(PointerEventData eventData){
		if (!GetComponent<Button> ().interactable) {
			GM.genie.bubble_text = "";
			return;
		}
		if (mod.active) {
			GM.genie.bubble_text = "If you don't want it, you can trade it back.\nNo return fees.";
			return;
		}
		if (buy && price >= GM.player.hp) {
			GM.genie.bubble_text = "If I sold this to you, you'd die,peasant.\nTry selling something else first.";
			return;
		}

		GM.genie.bubble_text = mod.GetDescription(buy, price);
		//GM.genie.bubble_text = "I'll " + (buy ? "Gain" : "Loose") + " " + Mathf.Abs(mod.value) + " " + price + " hitpoint " + (price == 1 ?  ;
	}
	public void OnPointerExit(PointerEventData eventData){
		GM.genie.bubble_text = "";
	}
}
