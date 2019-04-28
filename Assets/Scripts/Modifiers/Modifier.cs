using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : MonoBehaviour {

	public virtual string GetSpecDescription(){
		return "stat";
	}

	public virtual string GetDescription (bool buy, int price){
		string btext = "I can ";
		int abs_value = Mathf.Abs(value);
		if (buy) {
			btext += "increase your " + GetSpecDescription () + "\nby " + abs_value + " point" + (abs_value == 1 ? "" : "s") + " for " + price + " vitality.";
		} else {
			btext += "give you " + price + " vitality for taking " + price +  " \npoint" + (price == 1 ? "" : "s") + " from your " +  GetSpecDescription();
		}
		return btext + ".";
	}

	[SerializeField]
	public int value;
	public bool active = false;
	public abstract void Trigger ();
	public void Toggle(){
		Trigger ();
		active = !active;
		value *= -1;
	}

}
