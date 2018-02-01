using System.Collections;
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

		Sprite[] sprites = Resources.LoadAll<Sprite> ("");

		float imageWidth = 0f;
		float imageHeight = 0f;

		foreach (Sprite s in sprites) 
		{
			//Debug.Log (s.name + "|" + s.rect);
			if (s.rect.x + s.rect.width > imageWidth)
				imageWidth = s.rect.x + s.rect.width;

			if (s.rect.y + s.rect.height > imageHeight)
				imageHeight = s.rect.y + s.rect.height;
		}

		foreach (Sprite s in sprites) 
		{
			Vector2[] uvs = new Vector2[4];

			uvs [0] = new Vector2 (s.rect.x / imageWidth, s.rect.y / imageHeight);
			uvs [1] = new Vector2 ((s.rect.x + s.rect.width) / imageWidth, s.rect.y / imageHeight);
			uvs [2] = new Vector2 (s.rect.x / imageWidth, (s.rect.y + s.rect.height)/ imageHeight);
			uvs [3] = new Vector2 ((s.rect.x + s.rect.width) / imageWidth, (s.rect.y + s.rect.height) / imageHeight);

			tileUVMap.Add (s.name, uvs);

			Debug.LogFormat (s.name + ": " + uvs [0] + ", " + uvs [1] + ", " + uvs [2] + ", " + uvs [3]);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

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
}
