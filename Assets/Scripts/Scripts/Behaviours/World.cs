using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;

    public int width;
    public int height;

    public Tile[,] tiles;

    void Awake()
    {
        instance = this;
    }
	void Start ()
    {
        CreateTile();
        SubdivideTilesArray();
        
	}
	

	void Update () {
		
	}

    void CreateTile()
    {
        tiles = new Tile[width, height];

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j ++)
            {
                tiles[i, j] = new Tile(Tile.Type.Stone);
            }
        }
    }

    void SubdivideTilesArray( int i1 = 0, int i2 = 0)
    {
        //get size of segment
        int sizeX;
        int sizeY;

        if(tiles.GetLength(0) - i1 > 100)
        {
            sizeX = 100;
        }
        else
        {
            sizeX = tiles.GetLength(0) - i1;
        }
        if (tiles.GetLength(1) - i2 > 100)
        {
            sizeY = 100;
        }
        else
        {
            sizeY = tiles.GetLength(1) - i2;
        }

        GenerateMesh(i1, i2, sizeX, sizeY);

        if(tiles.GetLength(0) >= i1 + 100)
        {
            SubdivideTilesArray(i1 + 100, i2);
            return;
        }
        if(tiles.GetLength(1) >= i2 +100)
        {
            SubdivideTilesArray(0, i2 + 100);
            return;
        }
    }

    void GenerateMesh(int x, int y, int width, int height)
    {
        MeshData data = new MeshData(x, y, width, height);

        GameObject meshGO = new GameObject("CHUNK " + x + ", " + y);
        meshGO.transform.SetParent(this.transform);

        MeshFilter filter = meshGO.AddComponent < MeshFilter>();
        meshGO.AddComponent<MeshRenderer>();

        Mesh mesh = filter.mesh;

        mesh.vertices = data.vertices.ToArray();
        mesh.triangles = data.triangles.ToArray();        
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
