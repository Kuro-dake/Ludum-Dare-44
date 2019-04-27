using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {
	[SerializeField]
	Sprite[] sprites;
	[SerializeField]
	Color tile_color;
	[SerializeField]
	FloorTile[,] tiles;
	public void GenerateGrid(int size){
		tiles = new FloorTile[size, size];
		transform.DestroyChildren ();
		float tile_width = Setup.base_settings.GetFloat ("tile_width");
		float tile_height = Setup.base_settings.GetFloat ("tile_height");
		float offset_x = size * tile_width / -2f;
		float offset_y = size * tile_height / -2f;
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				GameObject tile = new GameObject ("tile " + x + ":" + y, new System.Type[]{ typeof(SpriteRenderer), typeof(FloorTile) });
				tile.sr ().sprite = sprites[Random.Range (0, sprites.Length)];
				tile.transform.position = new Vector2 (offset_x + x * tile_width, offset_y + y * tile_height);
				tile.transform.SetParent (transform);
				tile.sr ().sortingOrder = y * -1;
				tile.sr ().color = tile_color * Random.Range (.9f, 1.1f);
				tiles [x, y] = tile.GetComponent<FloorTile>();
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
}
