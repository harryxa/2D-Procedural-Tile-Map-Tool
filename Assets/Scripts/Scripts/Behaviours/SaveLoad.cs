using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


public static class SaveLoad
{    
	

//    public static void SaveMap()
//    {
//		string file_path = Application.dataPath + "/world.sav";
//		StreamWriter stream = new StreamWriter (file_path);
//
//		stream.
//
//		stream.Close ();
//
//		Debug.Log ("saved map to: " + file_path);
//
//
//    }
	public static void SaveMap()
	{

	}

//	public static void LoadMap()
//	{
//		string file_path = Application.dataPath + "/world.txt";
//
//		if(File.Exists(file_path))
//		{
//			BinaryFormatter bf = new BinaryFormatter();
//			StreamReader stream = new StreamReader(file_path, FileMode.Open);
//
//			//WorldData data = bf.Deserialize(stream) as WorldData;
//
//			stream.Close ();
//
//			//Debug.Log (data.worldTiles [0, 0].Y);
//		}
//	}
}

//[Serializable]
//public class WorldData
//{
//	public Tile[,] worldTiles;
//
//	public  WorldData()
//	{
//		for (int i = 0; i < World.instance.width; i++) {
//			for (int j = 0; j < World.instance.height; j++) {				
//				//initalise each tile in tiles array
//				worldTiles [i, j] =  World.instance.GetTileAt(i,j);
//				Debug.Log ("hello");
//			}
//		}
//	}
//}
