using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordButton : ShopButton {

	public override void Click ()
	{
		base.Click ();
		GM.sword = mod.active;
	}
}
