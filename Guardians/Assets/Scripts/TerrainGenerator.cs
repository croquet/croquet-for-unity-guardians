using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public float depth = 27.5f;// Yaxis
    
    public int width = 512;
    public int height = 512;

    private Terrain terrain;

    public float scale = 2.5f;
    void Start()
    {
        terrain = GetComponent<Terrain>(); 
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
        
    }

    void Update()
    {
    //    terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width*scale, depth, height*scale);
        terrainData.SetHeights(0,0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        Vector3 tPos = terrain.gameObject.transform.position;
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x+(int)tPos.x, y+(int)tPos.z); // Perlin Noise
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x * scale * .02f;
        float yCoord = (float)y * scale * .02f;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}
