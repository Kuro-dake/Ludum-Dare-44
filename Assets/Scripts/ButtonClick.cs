using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown(PointerEventData pointerEventData)
	{
		GM.sounds.Click ();
	}
}
