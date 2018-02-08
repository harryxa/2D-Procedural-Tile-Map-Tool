using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	public GameObject cursor;
	Vector3 lastMousePosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		//update cursor position
		Tile tileUnderMouse = GetTileAtWorldCoord(currMousePos);
		if (tileUnderMouse != null) {
			cursor.SetActive (true);
			Vector3 cursorPosition = new Vector3 (tileUnderMouse.X + 0.5f, tileUnderMouse.Y + 0.5f, 0);
			cursor.transform.position = cursorPosition;
		} else {
			cursor.SetActive (false);
		}

		// left click mouse
		if (Input.GetMouseButtonUp (0)) 
		{
			if (tileUnderMouse != null) {
				tileUnderMouse.type = Tile.Type.Void;
				Debug.Log (tileUnderMouse.chunkNumber);

			}
			World.instance.OnTileTypeChange (tileUnderMouse.chunkNumber);

		}

		//screen dragging using middle and right click
		if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) 
		{
			Vector3 diff = lastMousePosition - currMousePos;
			Camera.main.transform.Translate (diff);
		}

		lastMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	Tile GetTileAtWorldCoord(Vector3 coord)
	{
		int x = Mathf.FloorToInt (coord.x);
		int y = Mathf.FloorToInt (coord.y);

		return World.instance.GetTileAt (x, y);
	}

}
