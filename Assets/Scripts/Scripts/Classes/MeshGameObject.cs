using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGameObject : MonoBehaviour 
{
	int x;

	public int X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	int y;

	public int Y {
		get {
			return y;
		}
		set {
			y = value;
		}
	}

	int width;

	public int Width {
		get {
			return width;
		}
		set {
			width = value;
		}
	}

	int height;

	public int Height {
		get {
			return height;
		}
		set {
			height = value;
		}
	}
}
