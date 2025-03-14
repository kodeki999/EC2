using UnityEngine;

public class Tree : particles_ec_system
{
    MeshFilter plane;
    public GameObject surface;
    Vector3[] vertex;
    Vector2[] uvCoords;

    public override void ParticleEcSystem()
    {
        plane = surface.GetComponent<MeshFilter>();
        vertex = plane.mesh.vertices;
        uvCoords = plane.mesh.uv;

        for (int i = 0; i < vertex.Length; i++)
        {
            vertex[i] = surface.transform.TransformPoint(vertex[i]);
        }

        points = vertex;
        factor = uvCoords;
    }
}
