﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour 
{
	public static SpriteManager instance;

	Dictionary<string, Vector2[]> tileUVMap;

	void Awake () 
	{
		instance = this;
		tileUVMap = new Dictionary<string, Vector2[]> ();

		//load array of sprites from resource folder
		Sprite[] sprites = Resources.LoadAll<Sprite> ("");

		float imageWidth = 0f;
		float imageHeight = 0f;

		//finds the size of the sprite in image
		foreach (Sprite s in sprites) 
		{
			if (s.rect.x + s.rect.width > imageWidth)
				imageWidth = s.rect.x + s.rect.width;

			if (s.rect.y + s.rect.height > imageHeight)
				imageHeight = s.rect.y + s.rect.height;
		}

		//calculates uvs for texture
		foreach (Sprite s in sprites) 
		{
			Vector2[] uvs = new Vector2[4];

			uvs [0] = new Vector2 (s.rect.x / imageWidth, s.rect.y / imageHeight);
			uvs [1] = new Vector2 ((s.rect.x + s.rect.width) / imageWidth, s.rect.y / imageHeight);
			uvs [2] = new Vector2 (s.rect.x / imageWidth, (s.rect.y + s.rect.height)/ imageHeight);
			uvs [3] = new Vector2 ((s.rect.x + s.rect.width) / imageWidth, (s.rect.y + s.rect.height) / imageHeight);

			//returns  coords of tile uv map
			tileUVMap.Add (s.name, uvs);

			//Debug.LogFormat (s.name + ": " + uvs [0] + ", " + uvs [1] + ", " + uvs [2] + ", " + uvs [3]);
		}
	}


	//returns coords of uv map for tile 
	public Vector2[] GetTileUvs (Tile tile)
	{
		string key = tile.type.ToString ();

		if (tileUVMap.ContainsKey (key) == true) {
			return tileUVMap [key];
		} else {

			Debug.LogError ("No UV map for tile type: " + key);
			return tileUVMap ["Void"];
		}
	}



	public Vector2[] GetWallUVsAtQuadrant(Tile.Wall wall, int quadrant)
	{
		if (wall == Tile.Wall.Empty)
			return tileUVMap ["Empty"];

		string key = wall.ToString () + "_" + quadrant.ToString ();

		if (tileUVMap.ContainsKey (key) == true) {
			return tileUVMap [key];
		} else {

			Debug.LogError ("No UV map for tile type: " + key);
			return tileUVMap ["Void"];
		}

	}



}
