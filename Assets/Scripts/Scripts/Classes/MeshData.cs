using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<Vector3> vertices;
	public List<Vector2> UVs;
    public List<int> triangles;

    public MeshData (int x, int y, int width, int height)
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();
		UVs = new List<Vector2> ();

        for (int i = x; i < width + x;i++)
        {
            for (int j = y; j < height + y; j++)
            {
                CreateSquare(i, j);
            }
        }
    }

	//takes position of tile
    void CreateSquare ( int x, int y)
    {
		//gets tile at x, y coords
        Tile tile = World.instance.GetTileAt(x, y);

		//create square with vertices
        vertices.Add(new Vector3(x + 0, y + 0));
        vertices.Add(new Vector3(x + 1, y + 0));
        vertices.Add(new Vector3(x + 0, y + 1));
        vertices.Add(new Vector3(x + 1, y + 1));

        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 4);

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count - 4);

		//returns tile uv coords for that type of tile
		UVs.AddRange(SpriteManager.instance.GetTileUvs(tile));	
    }

	public void RewriteUV(int x, int y, int width, int height)
	{
		UVs = new List<Vector2> ();	

		for (int i = x; i < width + x; i++) 
		{
			for (int j = y; j < height + y; j++) 
			{
				Tile tile = World.instance.GetTileAt (i, j);
				UVs.AddRange (SpriteManager.instance.GetTileUvs (tile));
			}
		}
	}
}
