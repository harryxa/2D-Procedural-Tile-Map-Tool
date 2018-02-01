using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tile class represents all the tiles in the  world
public class Tile
{
	
    public enum Type { Dirt, Grass, Cobble, Water, Sand, Void }
    public Type type;

	//constructor to create and assign tile
    public Tile (Type type)
    {
		this.type = type;
    }
}
