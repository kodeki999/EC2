using UnityEngine;

public class MeshToTerrain : MonoBehaviour
{
    public Terrain terrain;
    public MeshFilter meshFilter;

    public void ConvertMeshToTerrain()
    {
        TerrainData terrainData = terrain.terrainData;
        Mesh mesh = meshFilter.sharedMesh;

        // Ottieni i vertici della mesh
        Vector3[] vertices = mesh.vertices;

        // Ottieni la risoluzione del Terrain
        int resolution = terrainData.heightmapResolution;
        float[,] heights = new float[resolution, resolution];

        // Dimensioni del terreno
        Vector3 terrainSize = terrainData.size;

        // Converte i vertici in altezze
        foreach (Vector3 vertex in vertices)
        {
            int x = Mathf.RoundToInt(vertex.x / terrainSize.x * resolution);
            int z = Mathf.RoundToInt(vertex.z / terrainSize.z * resolution);

            if (x >= 0 && x < resolution && z >= 0 && z < resolution)
            {
                heights[z, x] = vertex.y / terrainSize.y;
            }
        }

        // Assegna le altezze al Terrain
        terrainData.SetHeights(0, 0, heights);
    }
}