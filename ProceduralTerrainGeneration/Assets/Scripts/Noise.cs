﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{ 
    public enum NormalizeMode
    {
        Local,
        Global
    }

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1f;
        float frequency = 1f;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-10000,10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        float[,] noiseMap = new float[mapWidth,mapHeight];
        if(scale <= 0)
        {
            scale = 0.001f;
        }

        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2;
        float halfHeight = mapHeight / 2;

        for(int y = 0;  y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                amplitude = 1f;
                frequency = 1f;
                float noiseHeight = 0f;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) /scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) /scale * frequency ;
                    float perlinValue = Mathf.PerlinNoise(sampleX ,sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                if(noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight,0,int.MaxValue);
                }
            }
        }
        return noiseMap;
    }
}
