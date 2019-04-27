﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{

	int direction = 0;

	Dictionary<string, Pair<input_action, input_action>> key_mappings = new Dictionary<string, Pair<input_action, input_action>> () {
		/*{ "Fire3", new Pair<input_action, input_action> (input_action.none, input_action.punch) },
		{ "Fire4", new Pair<input_action, input_action> (input_action.none, input_action.kick) },
		{ "Fire1", new Pair<input_action, input_action> (input_action.none, input_action.block) },
		{ "Fire2", new Pair<input_action, input_action> (input_action.none, input_action.special) },*/
		{ "Horizontal", new Pair<input_action, input_action> (input_action.left, input_action.right) },
		{ "Vertical", new Pair<input_action, input_action> (input_action.down, input_action.up) },
	};
	Dictionary<input_action, int[]> movement_values = new Dictionary<input_action, int[]>(){
		{input_action.up, new int[]{0,1}},
		{input_action.down, new int[]{0,-1}},
		{input_action.left, new int[]{-1,0}},
		{input_action.right, new int[]{1,0}},
	};
	List<input_action> buttons_down = new List<input_action> ();
	List<input_action> movement = new List<input_action> (new input_action[]{ input_action.left, input_action.right, input_action.up, input_action.down });


	bool HandleButtonDown (input_action mapped_action)
	{

		if (buttons_down.Contains (mapped_action) || GM.routines.any_routines_running || !GM.inst.player_turn) {
			return false;
		}

		buttons_down.Add (mapped_action);
		if (movement.Contains (mapped_action)) {
			
			GM.player.Move (movement_values [mapped_action] [0], movement_values [mapped_action] [1]);
			GM.genie.SetActive (false);
		}

		return false;
	}

	void HandleButtonUp (input_action mapped_action)
	{

		if (!buttons_down.Contains (mapped_action)) {
			return;
		}

		buttons_down.Remove (mapped_action);

		if (movement.Contains (mapped_action)) {
			//GM.player.SetMovementDirection (0);
		}

	}



	void HandlePressedButtons ()
	{
		/*foreach (input_action a in buttons_down) {
		if (line_movement.Contains (a) && GM.player.SetCharacterMode(character_mode.moving)) {
			GM.player.LineMovement(a == input_action.up ? 1 : -1);
		}
	}*/
	}

	void Update ()
	{

		foreach (KeyValuePair<string, Pair<input_action, input_action>> kv in key_mappings) {
			int val = Input.GetAxis (kv.Key).SignInt();

			if (val == 0) {
				HandleButtonUp (kv.Value.first);
				HandleButtonUp (kv.Value.second);
				continue;
			}

			input_action passed_action = val > 0 ? kv.Value.second : kv.Value.first;
			//			Debug.Log (passed_action);
			if (HandleButtonDown (passed_action)) {
				//break;
			}
		}

		HandlePressedButtons ();


	}
}


public enum input_action
{
	none,
	punch,
	kick,
	block,
	special,
	down,
	up,
	left,
	right
}