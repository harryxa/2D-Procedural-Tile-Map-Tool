using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise 
{

	int seed;

	public int Seed {
		get {
			return seed;
		}
		set {
			seed = value;
		}
	}

	float frequency;

	public float Frequency {
		get {
			return frequency;
		}
		set {
			frequency = value;
		}
	}

	float amplitude;

	public float Amplitude {
		get {
			return amplitude;
		}
		set {
			amplitude = value;
		}
	}

	float lacunarity;

	public float Lacunarity {
		get {
			return lacunarity;
		}
		set {
			lacunarity = value;
		}
	}

	float persistance;

	public float Persistance {
		get {
			return persistance;
		}
		set {
			persistance = value;
		}
	}

	int octaves;

	public int Octaves {
		get {
			return octaves;
		}
		set {
			octaves = value;
		}
	}

	public Noise(int seed, float frequency, float amplitude, float lacunarity, float persistance, int octaves)
	{
		this.seed = seed;
		this.frequency = frequency;
		this.amplitude = amplitude;
		this.lacunarity = lacunarity;
		this.octaves = octaves;
	}

	public float[,] GetNoiseValues (int width, int height)
	{
		float[,] noiseValue = new float[width, height];


		float max = 0f;
		float min = float.MaxValue; //largest floating point possible


        for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				noiseValue [i, j] = 0f;

                //frequency and amplitude are modified within the loop so... 
                //are reassigned the correct value afterwards
				float tempA = amplitude;
				float tempF = frequency;

                //loop through octaves
				for (int k = 0; k < octaves; k++)
				{
                    //width and height cast to floats otherwise it returns an integar. The value will always return the same if so
					noiseValue [i, j] += Mathf.PerlinNoise ((i  + seed) / (float)width * frequency, j / (float)height * frequency) * amplitude;
                                        
                    frequency *= lacunarity;
					amplitude *= persistance;
				}
				amplitude = tempA;
				frequency = tempF;

				if (noiseValue [i, j] > max)
					max = noiseValue [i, j];
				if (noiseValue [i, j] < min)
					min = noiseValue [i, j];
			}
		}

        
		for(int i = 0; i <width; i++)
		{
			for(int j = 0; j <height; j++)
			{
                //sets noise values between 0 and 1, scaled appropriately
				noiseValue[i,j] = Mathf.InverseLerp(max, min, noiseValue[i,j]);
			}
		}
		return noiseValue;
	}
}
