﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
	[SerializeField]
	Sprite[] sprites;
	[SerializeField]
	public Color tile_color;
	[SerializeField]
	FloorTile[,] tiles;
	[SerializeField]
	Material light_material;
	public int grid_size_x, grid_size_y;
	public void GenerateGrid(int size_x, int size_y){
		grid_size_x = size_x;
		grid_size_y = size_y;

		tiles = new FloorTile[size_x, size_y];
		transform.DestroyChildren ();
		float tile_width = Setup.base_settings.GetFloat ("tile_width");
		float tile_height = Setup.base_settings.GetFloat ("tile_height");
		float offset_x = size_x * tile_width / -2f;
		float offset_y = size_y * tile_height / -2f;
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				GameObject tile = new GameObject ("tile " + x + ":" + y, new System.Type[]{ typeof(SpriteRenderer), typeof(FloorTile) });
				tile.sr ().sprite = sprites[Random.Range (0, sprites.Length)];
				tile.transform.position = new Vector2 (offset_x + x * tile_width, offset_y + y * tile_height);
				tile.transform.SetParent (transform);
				tile.sr ().sortingOrder = y * -1 + x;
				tile.sr ().color = tile_color * Random.Range (.9f, 1.1f);
				tile.sr ().sortingLayerName = "Ground";
				FloorTile ft = tile.GetComponent<FloorTile> ();
				ft.Init (x, y);
				//tile.sr ().material = light_material;
				tiles [x, y] = ft;
			}
		}
	}

	public int max_x {get{return tiles.GetLength (0) - 1;}}
	public int max_y {get{return tiles.GetLength (1) - 1;}}

	public FloorTile this[int x, int y]{
		get{
			x = Mathf.Clamp (x, 0, max_x);
			y = Mathf.Clamp (y, 0, max_y);


			return tiles [x, y];
		}
	}

	public FloorTile this[GridPosition gp]{
		get{
			return this [gp.x, gp.y];
		}
	}

	public List<GridPosition> GetUnoccupiedFieldPositions(){
		List<GridPosition> positions = new List<GridPosition> ();
		for (int x = 0; x < grid_size_x; x++) {
			for (int y = 0; y < grid_size_y; y++) {
				if (GM.characters [x, y] == null) {
					positions.Add (new GridPosition( x, y ));
				}
			}
		}
		return positions;
	}

	public bool[,] GetPassabilityMap(List<GridPosition> free = null){
		bool[,] ret = new bool[grid_size_x, grid_size_y];
		for (int x = 0; x < grid_size_x; x++) {
			for (int y = 0; y < grid_size_y; y++) {
				ret [x, y] = GM.characters [x, y] == null || free.Contains(new GridPosition(x,y));
			}
		}
		return ret;
	}


}
