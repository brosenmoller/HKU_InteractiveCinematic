using UnityEngine;

public class ComputedCaveGenerator : MonoBehaviour
{
    [Header("Compute")]
    [SerializeField] ComputeShader marchingCubesCompute;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField][Range(0, 1)] float surfaceLevel;
    public int chunkSize;
    public float squareSize;

    private Camera mainCamera;
    private float[] map;

    [ContextMenu("Setup")]
    public void Setup()
    {
        mainCamera = Camera.main;
        map = new float[chunkSize * chunkSize * chunkSize];

        for (int i = 0; i < chunkSize; i++)
        {
            map[i] = Random.Range(0.0f, 1.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse");
        }
    }

    private void AddTerrain()
    {

    }

    [ContextMenu("Generate")]
    // Source partially by Sebastion Lague https://github.com/SebLague/Marching-Cubes/blob/master/Assets/Scripts/MeshGenerator.cs
    public void GenerateMesh()
    {
        int numVoxelsPerAxis = chunkSize - 1;
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        ComputeBuffer mapBuffer = new(map.Length, sizeof(float));
        mapBuffer.SetData(map);
        ComputeBuffer trianglesBuffer = new(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
        ComputeBuffer triCountBuffer = new(1, sizeof(int), ComputeBufferType.Raw);
        trianglesBuffer.SetCounterValue(0);

        marchingCubesCompute.SetBuffer(0, "map", mapBuffer);
        marchingCubesCompute.SetBuffer(0, "triangles", trianglesBuffer);
        marchingCubesCompute.SetFloat("surfaceLevel", surfaceLevel);
        marchingCubesCompute.SetInt("mapSize", chunkSize);
        marchingCubesCompute.SetFloat("squareSize", squareSize);

        marchingCubesCompute.Dispatch(0, chunkSize / 8, chunkSize / 8, chunkSize / 8);

        ComputeBuffer.CopyCount(trianglesBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        Triangle[] tris = new Triangle[numTris];
        trianglesBuffer.GetData(tris, 0, 0, numTris);

        mapBuffer.Dispose();
        trianglesBuffer.Dispose();
        triCountBuffer.Dispose();

        Mesh mesh = new();

        Vector3[] vertices = new Vector3[numTris * 3];
        int[] triangles = new int[numTris * 3];

        for (int i = 0; i < numTris; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                triangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}

