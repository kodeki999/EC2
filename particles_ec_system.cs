using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class particles_ec_system : MonoBehaviour
{

    public struct Party
    {
        public Vector3 position;
        public float dir;
        public float scale;
        //public float lean;
        //public float noise;


        public Party(Vector3 pos, float d, float s)
        {
            position.x = pos.x;
            position.y = pos.y;
            position.z = pos.z;
            dir = d;
            scale = s;
            //lean = 0;
            //noise = Random.Range(0.5f, 1);
            //if (Random.value < 0.5f) noise = -noise;
        }

    }

    public Material material;
    //public ComputeShader shader;
    public GameObject particle;
    MeshFilter particleMf;
    Mesh particleMesh;

    int numOfParticles;
    public Vector2[] factor;
    const int SIZE_BOID = 5 * sizeof(float);

    ComputeBuffer particleBuffer;
    ComputeBuffer argsBuffer;
    uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
    Party[] ts;
    Bounds bounds;
    Vector3 meshSize;
    [HideInInspector] public Vector3[] points;
    //[Range(10, 90)]
    //public float maxLean = 25;
    //int timeID;
    //int groupSize;
    //int kernelLeanParticle;


    public virtual void ParticleEcSystem() // da sovrescrivere
    {
        points = null;
        factor = null;
    }


    void Start() // da mettere nel void start
    {
        ParticleEcSystem();
        particleMf = particle.GetComponent<MeshFilter>();
        particleMesh = particleMf.sharedMesh;

        numOfParticles = points.Length;
        ts = new Party[numOfParticles];

        InitParts();
        InitShader();

        bounds = GetBoundingBox(points);
        meshSize = particleMesh.bounds.size;
        bounds.Expand(meshSize * 2);
    }


    void InitParts()
    {
        for (int i = 0; i < numOfParticles; i++)
        {
            Vector3 pos = points[i];
            float rot = Random.Range(0.0f, 180);

            ts[i] = new Party(pos, rot, Random.Range(0.5f, 1f));
        }

    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(bounds.center, bounds.size);
    //    if (particle != null && points != null)
    //    {
    //        Gizmos.color = Color.green; // Imposta i punti in verde
    //        foreach (Vector3 pos in points)
    //        {
    //            Gizmos.DrawSphere(pos, 1.5f); // Disegna un piccolo punto per ogni albero
    //        }
    //    }
    //}


    void InitShader()
    {
        particleBuffer = new ComputeBuffer(numOfParticles, SIZE_BOID);
        particleBuffer.SetData(ts);

        //uint threadGroupSize;
        //shader.GetKernelThreadGroupSizes(kernelLeanParticle, out threadGroupSize, out _, out _);
        //groupSize = Mathf.CeilToInt((float)numOfParticles / (float)threadGroupSize);


        //Initialize args buffer
        argsBuffer = new ComputeBuffer(1, 5 * sizeof(uint), ComputeBufferType.IndirectArguments);

        if (particle != null)
        {
            args[0] = (uint)particleMesh.GetIndexCount(0);
            args[1] = (uint)numOfParticles;
        }


        //kernelLeanParticle = shader.FindKernel("LeanParticle");
        argsBuffer.SetData(args);
        material.SetBuffer("ParticleBuffer", particleBuffer);

        //shader.SetBuffer(kernelLeanParticle, "particleBuffer", particleBuffer);
        //shader.SetFloat("maxLean", maxLean * Mathf.PI / 180);
        //timeID = Shader.PropertyToID("time");


    }


    void OnDestroy() // da mettere nel void OnDestroy
    {
        if (particleBuffer != null)
        {
            particleBuffer.Dispose();
        }

        if (argsBuffer != null)
        {
            argsBuffer.Dispose();
        }
    }

    // Update is called once per frame
    void Update() // da mettere nel void update
    {
        if (particleMesh != null)
        {

            //shader.SetFloat(timeID, Time.time);
            //shader.Dispatch(kernelLeanParticle, groupSize, 1, 1);
            Graphics.DrawMeshInstancedIndirect(particleMesh, 0, material, bounds, argsBuffer);
        }

    }

    public static Bounds GetBoundingBox(Vector3[] ps)
    {


        // Inizializza il bounding box con il primo punto
        Vector3 min = ps[0];
        Vector3 max = ps[0];

        // Trova i valori minimi e massimi
        foreach (Vector3 p in ps)
        {
            min = Vector3.Min(min, p);
            max = Vector3.Max(max, p);
        }

        // Crea il bounding box
        Vector3 center = (min + max) / 2f;
        Vector3 size = max - min;
        return new Bounds(center, size);
    }
}
