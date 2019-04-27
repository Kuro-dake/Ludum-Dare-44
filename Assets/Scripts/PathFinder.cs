using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class PathSearch {

	private static bool[,] map;
	private static int[,] scores;


	public static List<GridPosition> FindDirectPath(GridPosition from, GridPosition to, List<GridPosition> free = null){
		List<GridPosition> path = FindPathBlocks (from, to, free);

		return path;

	}

	public static List<GridPosition> FindPathBlocks(GridPosition from, GridPosition to, List<GridPosition> free = null){
		List<GridPosition> g_path = new List<GridPosition> ();

		if (from == to) {
			return g_path;
		}
		if (free == null) {
			free = new List<GridPosition>();
		}
		free.Add (from);

		RateBlocks (new List<GridPosition>(new GridPosition[]{from}), to, 0, free);

		if (scores [to.x, to.y] == 30000) {
			return g_path;
		}


		GridPosition current_gp = to;
		g_path.Add (current_gp);

		while (current_gp != from) {
			int min_score = 30000;

			foreach(GridPosition dgp in current_gp.all_directions){

				if (dgp.x >= scores.GetLength (0) || dgp.x < 0 || dgp.y >= scores.GetLength (1) || dgp.y < 0 || !map [dgp.x, dgp.y]) {
					continue;
				}

				if(min_score > scores[dgp.x, dgp.y]){
					min_score = scores[dgp.x, dgp.y];
					current_gp = dgp;
				}

			}

			g_path.Add(current_gp);

		}
		g_path.Remove (from);
		g_path.Reverse ();

		return g_path;



	}

	public static int iterations = 0;
	public static int max_value = 0;
	public static bool finish = false;

	public static List<GridPosition> rated = new List<GridPosition>();
	public static int limit_depth = -1;
	public static bool use_visibility_map = false;
	public static bool get_all = false;

	static List<GridPosition> GetScoredRangeBlocks(GridPosition from, int range){
		List<GridPosition> ret = new List<GridPosition>();

		int start_x = Mathf.Clamp( from.x - range, 0 , scores.GetLength(0));
		int end_x = Mathf.Clamp( from.x + range,0 , scores.GetLength(0) );

		int start_y = Mathf.Clamp( from.y - range, 0 , scores.GetLength(1));
		int end_y = Mathf.Clamp( from.y + range,0 , scores.GetLength(1) );

		for (int x = start_x; x < end_x; x++) {
			for (int y = start_y; y < end_y; y++) {
				if(scores[x,y] != 30000){
					ret.Add(new GridPosition(x,y));
				}

			}
		}

		return ret;
	}

	public static List<GridPosition> GetNearbyBlocks(GridPosition from, int range = 10){
		if (range == 0) {
			return new List<GridPosition>();
		}
		limit_depth = range;
		get_all = true;
		RateBlocks (new List<GridPosition> (new GridPosition[]{from}), null);
		limit_depth = -1;
		get_all = false;

		return GetScoredRangeBlocks(from, range);
	}

	public static List<GridPosition> GetAccessibleBlocks(GridPosition from, int range = 10){

		if (range == 0) {
			return new List<GridPosition>();
		}
		limit_depth = range;
		use_visibility_map = true;
		RateBlocks (new List<GridPosition> (new GridPosition[]{from}), null);
		limit_depth = -1;
		use_visibility_map = false;

		return GetScoredRangeBlocks(from, range);
	}

	public static void RateBlocks(List<GridPosition> to_rate, GridPosition target, int value = 0, List<GridPosition> free = null){
		if (value == 0) {
			iterations = 0;
			scores = new int[GM.floor.grid_size,GM.floor.grid_size];

			map = GM.floor.GetPassabilityMap(free);


			GridPosition origin = to_rate[0];
			rated.Add(origin);
			scores[origin.x, origin.y] = 0;
			rated.Clear();
			finish = false;
		}

		if (finish || limit_depth > 0 && value > limit_depth) {

			return;
		}

		List<GridPosition> rated_next = new List<GridPosition> ();
		foreach (GridPosition gp in to_rate) {

			foreach(GridPosition dir_gp in gp.all_directions){
				iterations++;

				if(target != null && dir_gp == target){
					finish = true;
				}

				if (dir_gp.x >= scores.GetLength (0) || dir_gp.x < 0 || dir_gp.y >= scores.GetLength (1) || dir_gp.y < 0 ) {
					continue;
				}

				if(!map [dir_gp.x, dir_gp.y]){
					if(use_visibility_map){
						scores[dir_gp.x,dir_gp.y] = 1;
					}
					continue;
				}

				int increased_val = scores[gp.x, gp.y] + ((dir_gp - gp).abs_sum == 2 ? 14 : 10);
				scores[dir_gp.x, dir_gp.y] = scores[dir_gp.x, dir_gp.y] > increased_val ? increased_val : scores[dir_gp.x, dir_gp.y];
				if(!rated.Contains(dir_gp)){
					rated_next.Add(dir_gp);
					rated.Add(dir_gp);
				}
			}






		}
		if (rated_next.Count > 0) {
			RateBlocks (rated_next, target, value + 1, free);
		}



	}

}