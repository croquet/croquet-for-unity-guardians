using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int depth = 20;// Yaxis
    
    public int width = 256;
    public int height = 256;

    private Terrain terrain;

    public float scale = 20.0f;
    void Start()
    {
        terrain = GetComponent<Terrain>(); 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        
    }

    void Update()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0,0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y); // Perlin Noise
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
