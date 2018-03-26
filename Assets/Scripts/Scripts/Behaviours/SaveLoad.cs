using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
	public Tile currentTile;

	char tileTypeChar;
	string fileString = "";
	public const string fileName = "LatestSave.txt";

	public void CreateTextFile()	{

		StreamWriter sr = File.CreateText(fileName);
		sr.WriteLine("Save File: ");

		for (int j = World.instance.height - 1; j >= 0 ; j--)
		{		
			for (int i = 0; i < World.instance.width; i++)
			{
				currentTile = new Tile(World.instance.GetTileAt(i, j).type);

				switch (currentTile.type)
				{
				case Tile.Type.Dirt:
					tileTypeChar = 'D';
					break;
				case Tile.Type.Grass:
					tileTypeChar = 'G';
					break;
				case Tile.Type.Smooth_Stone:
					tileTypeChar = 'C';
					break;
				case Tile.Type.Deep_Water:
					tileTypeChar = 'W';
					break;
				case Tile.Type.Shallow_Water:
					tileTypeChar = 'w';
					break;
				case Tile.Type.Sand:
					tileTypeChar = 'S';
					break;
				case Tile.Type.Tree:
					tileTypeChar = 'T';
					break;
				case Tile.Type.Void:
					tileTypeChar = 'X';
					break;
				default:
					break;
				}

				fileString = fileString + tileTypeChar + " ";
			}
			sr.WriteLine(fileString);
			fileString = "";
		}

		//sr.WriteLine("I can write ints {0} or floats {1}, and so on.", 1, 4.2);
		sr.Close();
	}
}
