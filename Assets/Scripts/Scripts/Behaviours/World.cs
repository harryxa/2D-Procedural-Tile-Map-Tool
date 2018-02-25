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
	public int chunkSize = 50;

    public Tile[,] tiles;

	MeshData data;
    MeshData MountainData;
	int meshGOvalue = 0;
	int meshGOMountainvalue = 0;

	public bool randomiseMap = false;
	public string seed;
	public bool randomSeed;

	public float frequency;
	public float amplitude;
	public float lacunarity;
	public float persistance;
	public int octaves;

	public float deepWaterEnd;

	float shallowWaterStart;
	public float shallowWaterEnd;

	float sandStartHeight;
	public float sandEndHeight;

	float dirtStartHeight;
	public float dirtEndHeight;

	float grassStartHeight;
	public float grassEndHeight;

	float cobbleStartHeight;
	public float cobbleEndHeight;

	public float mountainStartHeight;

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

		shallowWaterStart = deepWaterEnd;
		sandStartHeight = shallowWaterEnd;
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

		SubdivideMountainArray ();
	}
	

	void Update () 
	{
		if (randomiseMap == true) {
			RandomizeMap ();
			randomiseMap = false;
		}
	}

	//redraws chunk dependant on the chunk number
	public void OnTileTypeChange (int chunkNumber, int MountainChunkNumber)
	{
       //update chunk mesh
		GameObject chunk = GameObject.Find ("CHUNK " + chunkNumber);
		MeshFilter chunkFilter = chunk.GetComponent<MeshFilter> ();
		Mesh chunkMesh = chunkFilter.mesh;
		MeshGameObject meshScript = chunk.GetComponent<MeshGameObject> ();

		data.RewriteUV (meshScript.X, meshScript.Y, meshScript.Width, meshScript.Height);
		chunkMesh.uv = data.UVs.ToArray ();

        //update mountainlayer mesh
        GameObject mountainChunk = GameObject.Find("MountainLayer " + MountainChunkNumber);
        MeshFilter mountainFilter = mountainChunk.GetComponent<MeshFilter>();
        Mesh mountainMesh = mountainFilter.mesh;
        MeshGameObject mountainMeshScript = mountainChunk.GetComponent<MeshGameObject>();

        MountainData.RewriteMountainUV(mountainMeshScript.X, mountainMeshScript.Y, mountainMeshScript.Width, mountainMeshScript.Height);
        mountainMesh.uv = MountainData.UVs.ToArray();
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
       	 	}
    	}
	}

	Tile SetTileAtHeight(float currentHeight, Tile tile = null)
	{
		//if tile hasnt been initialised return new tile type

		if (tile == null) 
		{
			if (currentHeight <= deepWaterEnd) {
				return new Tile (Tile.Type.Deep_Water);
			}
			if (currentHeight >= shallowWaterStart && currentHeight <= shallowWaterEnd) {
				return new Tile (Tile.Type.Shallow_Water);
			}
			if (currentHeight >= sandStartHeight && currentHeight <= sandEndHeight) {
				return new Tile (Tile.Type.Sand);
			}
			if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight) {
				return new Tile (Tile.Type.Dirt);
			}
			if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight) {
				//TODO: repeat changes in else
				return new Tile (Tile.Type.Grass);
			} 
			if (currentHeight >= cobbleStartHeight && currentHeight <= cobbleEndHeight) {
				if (currentHeight >= mountainStartHeight) {
					return new Tile (Tile.Type.Smooth_Stone, Tile.Wall.Brick);
				}

				return new Tile (Tile.Type.Smooth_Stone);
			}			
			return new Tile (Tile.Type.Void);
		} 
		//else change tile type
		else {
			if (currentHeight <= deepWaterEnd){
				tile.type = Tile.Type.Deep_Water;
			} else if (currentHeight >= shallowWaterStart && currentHeight <= shallowWaterEnd){
				tile.type = Tile.Type.Shallow_Water;			
			} else if (currentHeight >= sandStartHeight && currentHeight <= sandEndHeight) {
				tile.type = Tile.Type.Sand;
			} else if (currentHeight >= dirtStartHeight && currentHeight <= dirtEndHeight) {
				tile.type = Tile.Type.Dirt;				
			} else if (currentHeight >= grassStartHeight && currentHeight <= grassEndHeight) {
				tile.type = Tile.Type.Grass;

			} else if (currentHeight >= cobbleStartHeight && currentHeight <= cobbleEndHeight) 
			{
				if (currentHeight >= mountainStartHeight) 
				{
					tile.wall = Tile.Wall.Brick;
				}
				
				tile.type = Tile.Type.Smooth_Stone;
			} 
			else {			
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
		for (int i = 0; i < meshGOMountainvalue; i++)
        {
            for(int j = 0; j < meshGOMountainvalue; j++)
            {
                OnTileTypeChange(i, j);
            }
        }
	}

	//GROUND TILES
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
        GenerateTilesLayer(index1, index2, sizeX, sizeY);

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
    void GenerateTilesLayer(int x, int y, int width, int height)
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


	//MOUNTAIN TILES
	void SubdivideMountainArray( int index1 = 0, int index2 = 0)
	{
		//get size of chunk
		int sizeX;
		int sizeY;

		//x axis

		//if tiles along x - start of chunk > chunk size
		if(tiles.GetLength (0) - index1 > 25)
		{
			sizeX = 25;
		}
		else
		{
			sizeX = tiles.GetLength(0) - index1;
		}
		//y axis
		if (tiles.GetLength(1) - index2 > 25)
		{
			sizeY = 25;
		}
		else
		{
			sizeY = tiles.GetLength(1) - index2;
		}

		//generate 100 x 100 tile mesh 
		GenerateMountainLayer(index1, index2, sizeX, sizeY);

		if(tiles.GetLength(0) >= index1 + 25)
		{
			SubdivideMountainArray(index1 + 25, index2);
			return;
		}
		if(tiles.GetLength(1) >= index2 +25)
		{
			SubdivideMountainArray(0, index2 + 25);
			return;
		}
	}

	//create mesh from MeshData
	void GenerateMountainLayer(int x, int y, int width, int height)
	{
		//create mesh at coords of 100 x 100 tiles
		MountainData = new MeshData(x, y, width, height, true);

		//new chunk gameobject, child of world, with id
		GameObject meshGO = new GameObject("MountainLayer " + meshGOMountainvalue);

		MeshGameObject mountainMeshScript = meshGO.AddComponent<MeshGameObject> ();

        mountainMeshScript.X = x;
        mountainMeshScript.Y = y;
        mountainMeshScript.Width = width;
        mountainMeshScript.Height = height;

		meshGO.transform.SetParent(this.transform);

		//add a mesh filter and renderer
		MeshFilter filter = meshGO.AddComponent <MeshFilter>();
		MeshRenderer render = meshGO.AddComponent<MeshRenderer>();
		render.material = material;

		Mesh mesh = filter.mesh;

		//create vertices, triangles and uvs from meshdata
		mesh.vertices = MountainData.vertices.ToArray();
		mesh.triangles = MountainData.triangles.ToArray();

		mesh.uv = MountainData.UVs.ToArray ();

		for (int i = x; i <  x + width; i++) 
		{
			for (int j = y; j < y + height; j++) 
			{
				tiles [i, j].MountainChunkNumber = meshGOMountainvalue;
			}
		}

		meshGOMountainvalue++;
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

	public Tile[] GetNeighbours(int x, int y, bool diagonals = false)
	{
		//conditional operator(-? x : y) for reference
		//same as:
		//if (diagonals)
		//	return 8;
		//else return 4;

		Tile[] neighbours = new Tile[diagonals? 8 : 4];

		// N E S W
		neighbours[0] = GetTileAt(x + 0, y + 1);
		neighbours[1] = GetTileAt(x + 1, y + 0);
		neighbours[2] = GetTileAt(x + 0, y - 1);
		neighbours[3] = GetTileAt(x - 1, y + 0);

		//NE SE SW NW
		if (diagonals == true) 
		{
			neighbours[4] = GetTileAt(x + 1, y + 1);
			neighbours[5] = GetTileAt(x + 1, y - 1);
			neighbours[6] = GetTileAt(x - 1, y - 1);
			neighbours[7] = GetTileAt(x - 1, y + 1);
		}

		return neighbours;

	}

}
