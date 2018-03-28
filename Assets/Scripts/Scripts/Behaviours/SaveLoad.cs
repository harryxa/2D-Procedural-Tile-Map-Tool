using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
	public Tile currentTile;

	string tileTypeChar = "";
	string fileString = "";
	public const string fileName = "Save.txt";

	public void CreateTextFile()
    {
		StreamWriter sr = File.CreateText(fileName);		

		for (int x = 0; x < World.instance.width ; x++)
		{		
			for (int y = 0; y < World.instance.height; y++)
			{
				currentTile = new Tile(World.instance.GetTileAt(x, y).type);

				switch (currentTile.type)
				{
				case Tile.Type.Dirt:
					tileTypeChar = "D";
					break;
				case Tile.Type.Grass:
					tileTypeChar = "G";
					break;
				case Tile.Type.Smooth_Stone:
					tileTypeChar = "C";
					break;
				case Tile.Type.Deep_Water:
					tileTypeChar = "W";
					break;
				case Tile.Type.Shallow_Water:
					tileTypeChar = "w";
					break;
				case Tile.Type.Sand:
					tileTypeChar = "S";
					break;
				case Tile.Type.Tree:
					tileTypeChar = "T";
					break;
				case Tile.Type.Void:
					tileTypeChar = "X";
					break;
				default:
					break;
				}

                fileString = tileTypeChar;
                sr.WriteLine(fileString);
			}
			fileString = "";
		}
		sr.Close();
	}

    private float loadTileString;
    public Tile loadTile;

    protected FileInfo theSourceFile = null;
    protected StreamReader reader = null;
    string text = ""; //assigned to allow first line to be read below

    public void LoadLevel()
    {
        theSourceFile = new FileInfo("Save.txt");
        reader = theSourceFile.OpenText();


        for (int i = 0; i < World.instance.width; i++)
        {
            for (int j = 0; j < World.instance.height; j++)
            {
                text = reader.ReadLine();

                if (text == "D")
                {
                    loadTileString = World.instance.dirtEndHeight;
                }
                else if (text == "G")
                {
                    loadTileString = World.instance.grassEndHeight;
                }
                else if (text == "S")
                {
                    loadTileString = World.instance.sandEndHeight;
                }
                else if (text == "W")
                {
                    loadTileString = World.instance.deepWaterEnd;
                }
                else if (text == "C")
                {
                    loadTileString = World.instance.cobbleEndHeight;
                }
                else if (text == "w")
                {
                    loadTileString = World.instance.shallowWaterEnd;
                }
                else if (text == "T")
                {
                    loadTileString = World.instance.treeEndHeight;
                }
                else
                {
                    loadTileString = 1.5f;
                }

                //tiles[i, j] = MakeTileAtHeight(noiseValues[i, j]);
                World.instance.tiles[i, j] = World.instance.SetTileAtHeight(loadTileString);

                World.instance.tiles[i, j].X = i;
                World.instance.tiles[i, j].Y = j;
            }
        }

        World.instance.LoadMap();

        text = " ";
        reader.Close();
    }

}
