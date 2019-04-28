using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldButton : ShopButton {

	public override void Click ()
	{
		base.Click ();
		GM.shield = mod.active;
	}
}
