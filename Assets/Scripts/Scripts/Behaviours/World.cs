using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;

	public Material material;

	//determines size of mesh
    public int width;
    public int height;


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

	public float sandStartHeight;
	public float sandEndHeight;

	public float dirtStartHeight;
	public float dirtEndHeight;

	public float grassStartHeight;
	public float grassEndHeight;

	public float cobbleStartHeight;
	public float cobbleEndHeight;

	Noise noise;

    void Awake()
    {
        instance = this;

		if (randomSeed == true) {
			int value = Random.Range (-10000, 10000);
			seed = value.ToString ();
		}

		noise = new Noise (seed.GetHashCode (), frequency, amplitude, lacunarity, persistance, octaves);
    }
	void Start ()
    {
		//creates array of tiles with a type dependant on perlin noise
        CreateTile();
		//divide tile array into increments to create 100 x 100 tile mesh
        SubdivideTilesArray();
        
	}
	
	float randomizeTileTimer= 2f;

	void Update () 
	{
		randomizeTileTimer -= Time.deltaTime;
		if (randomizeTileTimer < 0)
		{
			//seaLevel = 5f;
			RandomizeMap ();
			randomizeTileTimer = 2f;

		}
		OnTileTypeChange ();
	}

	void OnTileTypeChange()
	{
		List<GameObject> chunk = new List<GameObject>();

		for (int i = 0; i < meshGOvalue; i++) 
		{
			chunk.Add(GameObject.Find("CHUNK " + i));

			MeshFilter filter = chunk [i].GetComponent<MeshFilter> ();
			Mesh mesh = filter.mesh;

			MeshGameObject meshScript = chunk [i].GetComponent<MeshGameObject> ();

			data.RewriteUV (meshScript.X, meshScript.Y, meshScript.Width, meshScript.Height);
			mesh.uv = data.UVs.ToArray ();

	
		}


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
       	 	}
    	}
	}

	Tile SetTileAtHeight(float currentHeight)
	{
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

	void RandomizeMap()
	{
		
		int value = Random.Range (-10000, 10000);

		noise.Seed = value;

		float[,] noiseValues = noise.GetNoiseValues (width, height);

		for (int i = 0; i < width; i++)
		{
			for(int j = 0; j < height; j ++)
			{
				//initalise each tile in tiles array
				tiles[i,j] = SetTileAtHeight(noiseValues[i,j]);           
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
        if(tiles.GetLength(0) - index1 > 100)
        {
            sizeX = 100;
        }
        else
        {
            sizeX = tiles.GetLength(0) - index1;
        }
		//y axis
        if (tiles.GetLength(1) - index2 > 100)
        {
            sizeY = 100;
        }
        else
        {
            sizeY = tiles.GetLength(1) - index2;
        }

		//generate 100 x 100 tile mesh 
        GenerateMesh(index1, index2, sizeX, sizeY);

        if(tiles.GetLength(0) >= index1 + 100)
        {
            SubdivideTilesArray(index1 + 100, index2);
            return;
        }
        if(tiles.GetLength(1) >= index2 +100)
        {
            SubdivideTilesArray(0, index2 + 100);
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
		meshGOvalue++;

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
    }

    public Tile GetTileAt (int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return tiles[x, y];
    }
}
