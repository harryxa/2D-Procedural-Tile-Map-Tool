﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;

	public Material material;

	//determines size of mesh
    public int width;
    public int height;
	public int chunkSize = 50;

    public Tile[,] tiles;

	MeshData data;
	int meshGOvalue = 0;

	public string seed;
	public bool randomSeed;

	public float frequency;
	public float amplitude;
	public float lacunarity;
	public float persistance;
	public int octaves;

	public float seaLevel;

	float sandStartHeight;
	public float sandEndHeight;

	float dirtStartHeight;
	public float dirtEndHeight;

	float grassStartHeight;
	public float grassEndHeight;

	float cobbleStartHeight;
	public float cobbleEndHeight;

	Noise noise;

    void Awake()
    {
		if (instance != null) {
			Debug.Log ("more instances of world, why?");
		}
        instance = this;

		if (randomSeed == true) {
			int value = Random.Range (-10000, 10000);
			seed = value.ToString ();
		}

		noise = new Noise (seed.GetHashCode (), frequency, amplitude, lacunarity, persistance, octaves);

		sandStartHeight = seaLevel;
		dirtStartHeight = sandEndHeight;
		grassStartHeight = dirtEndHeight;
		cobbleStartHeight = grassEndHeight;

    }
	void Start ()
    {
		//creates array of tiles with a type dependant on perlin noise
        CreateTile();
		//divide tile array into increments to create 100 x 100 tile mesh
        SubdivideTilesArray();        
	}
	
	float randomizeTileTimer= 0f;

	void Update () 
	{
		randomizeTileTimer -= Time.deltaTime;
		if (randomizeTileTimer < 0)
		{
			//seaLevel = 5f;
			//RandomizeMap ();

			randomizeTileTimer = 0f;

			//causing huge fps drop
			//OnTileTypeChange ();
		}
	}

	//TODO: redraw mesh only when necessary
	public void OnTileTypeChange (int chunkNumber)
	{

		GameObject chunk = GameObject.Find ("CHUNK " + chunkNumber);
		MeshFilter filter = chunk.GetComponent<MeshFilter> ();
		Mesh mesh = filter.mesh;

		MeshGameObject meshScript = chunk.GetComponent<MeshGameObject> ();


		//if(chunk.changed = true)
		data.RewriteUV (meshScript.X, meshScript.Y, meshScript.Width, meshScript.Height);
		mesh.uv = data.UVs.ToArray ();	

	}

	//create an array of tiles with a type
    void CreateTile()
    {

        tiles = new Tile[width, height];

		//returns a perlin noise value
		float[,] noiseValues = noise.GetNoiseValues (width, height);

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j ++)
			{
				
				//initalise each tile in tiles array
				tiles[i,j] = SetTileAtHeight(noiseValues[i,j]);
				tiles [i, j].X = i;
				tiles [i, j].Y = j;


				//tiles[i,j].RegisterTileTypeChanged((tile) => {OnTileTypeChange();});
       	 	}
    	}
		//tiles [49, 49].type = Tile.Type.Void;
	}

	Tile SetTileAtHeight(float currentHeight, Tile tile = null)
	{
		//if tile hasnt been initialised return new tile type

		if (tile == null) {
			if (currentHeight <= seaLevel)
				return new Tile (Tile.Type.Water);

			if (currentHeight >= sandStartHeight && currentHeight <= sandEndHeight)
				return new Tile (Tile.Type.Sand);

			if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight)
				return new Tile (Tile.Type.Dirt);

			if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight)
				return new Tile (Tile.Type.Grass);

			if (currentHeight >= cobbleStartHeight && currentHeight <= cobbleEndHeight)
				return new Tile (Tile.Type.Cobble);

			return new Tile (Tile.Type.Void);
		} 
		//else change tile type
		else {
			if (currentHeight <= seaLevel) {
				tile.type = Tile.Type.Water;
			} else if (currentHeight >= sandStartHeight && currentHeight <= sandEndHeight) {
				tile.type = Tile.Type.Sand;
			} else if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight) {
				tile.type = Tile.Type.Dirt;				
			} else if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight) {
				tile.type = Tile.Type.Grass;

			} else if (currentHeight >= cobbleStartHeight && currentHeight <= cobbleEndHeight) {
				tile.type = Tile.Type.Cobble;
			} else {			
				tile.type = Tile.Type.Void;	
			}
			return tile;
		}
	}

	void RandomizeMap()
	{		
		int value = Random.Range (-10000, 10000);
		noise.Seed = value;
		noise.Frequency = frequency;
		noise.Amplitude = amplitude;
		noise.Lacunarity = lacunarity;
		noise.Persistance = persistance;
		noise.Octaves = octaves;

		float[,] noiseValues = noise.GetNoiseValues (width, height);

		for (int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j ++)
			{
				//initalise each tile in tiles array
				tiles[i,j] = SetTileAtHeight(noiseValues[i,j], tiles[i,j]);           
			}
		}	
	}

	//divide tile array into increments to create 100 x 100 mesh
	void SubdivideTilesArray( int index1 = 0, int index2 = 0)
    {
        //get size of chunk
        int sizeX;
        int sizeY;

		//x axis

		//if tiles along x - start of chunk > chunk size
        if(tiles.GetLength (0) - index1 > chunkSize)
        {
			sizeX = chunkSize;
        }
        else
        {
            sizeX = tiles.GetLength(0) - index1;
        }
		//y axis
		if (tiles.GetLength(1) - index2 > chunkSize)
        {
			sizeY = chunkSize;
        }
        else
        {
            sizeY = tiles.GetLength(1) - index2;
        }

		//generate 100 x 100 tile mesh 
        GenerateMesh(index1, index2, sizeX, sizeY);

		if(tiles.GetLength(0) >= index1 + chunkSize)
        {
			SubdivideTilesArray(index1 + chunkSize, index2);
            return;
        }
		if(tiles.GetLength(1) >= index2 +chunkSize)
        {
			SubdivideTilesArray(0, index2 + chunkSize);
            return;
        }
    }

	//create mesh from MeshData
    void GenerateMesh(int x, int y, int width, int height)
    {
		//create mesh at coords of 100 x 100 tiles
		data = new MeshData(x, y, width, height);

		//new chunk gameobject, child of world, with id

		GameObject meshGO = new GameObject("CHUNK " + meshGOvalue);


		MeshGameObject meshScript = meshGO.AddComponent<MeshGameObject> ();

		meshScript.X = x;
		meshScript.Y = y;
		meshScript.Width = width;
		meshScript.Height = height;

        meshGO.transform.SetParent(this.transform);

		//add a mesh filter and renderer
		MeshFilter filter = meshGO.AddComponent <MeshFilter>();
		MeshRenderer render = meshGO.AddComponent<MeshRenderer>();
		render.material = material;

        Mesh mesh = filter.mesh;

		//create vertices, triangles and uvs from meshdata
		mesh.vertices = data.vertices.ToArray();
        mesh.triangles = data.triangles.ToArray();

		mesh.uv = data.UVs.ToArray ();

		for (int i = x; i <  x + width; i++) 
		{
			for (int j = y; j < y + height; j++) 
			{
				tiles [i, j].ChunkNumber = meshGOvalue;
			}
		}

		meshGOvalue++;
    }

    public Tile GetTileAt (int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
			//Debug.Log ("Tile (" + x + ", " + y + ") is out of range");
            return null;
        }
        return tiles[x, y];
    }


}
