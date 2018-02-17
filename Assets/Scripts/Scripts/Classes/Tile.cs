﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//tile class represents all the tiles in the  world
public class Tile
{
	
    public enum Type { Dirt, Grass, Smooth_Stone, Deep_Water, Shallow_Water, Sand, Void }
	public Type type;

	public enum Wall { Brick, Empty }
	public Wall wall;



	public int chunkNumber;
	public bool changed = false;

	InstalledObjects installedObjects;

	//constructor to create and assign tile
	public Tile (Type type, Wall wall = Wall.Empty)
	{
		this.type = type;
		this.wall = wall;
	}

	public int ChunkNumber {
		get {
			return chunkNumber;
		}
		set {
			chunkNumber = value;
		}
	}

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


}
