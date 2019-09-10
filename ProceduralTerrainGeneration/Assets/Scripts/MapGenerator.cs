﻿using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, Colored}
    public DrawMode drawMode;
    public int mapWidth, mapHeight;
    public float noiseScale;
    public bool autoUpdate;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colorMap[mapWidth * y + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colorMap, mapWidth, mapHeight));
        }
    }

    private void OnValidate()
    {
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }        
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
