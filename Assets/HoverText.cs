using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public void OnPointerEnter(PointerEventData eventData){
		GM.genie.bubble_text = "Ready to go peasant?";
	}
	public void OnPointerExit(PointerEventData eventData){
		GM.genie.bubble_text = "";
	}
}
