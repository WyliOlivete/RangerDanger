using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlaneGenerator : MonoBehaviour
{
    public float size = 1;
    public float power = 3, scale = 1, timeScale = 1;
    public int gridSize = 16;

    private float xOffset, yOffset;
    private MeshFilter filter;
    private MeshFilter mf;

    private void Start()
    {
        filter = GetComponent<MeshFilter>();
        filter.mesh = GenerateMesh();

        mf = GetComponent<MeshFilter>();
        MakeNoise();
    }

    private void Update()
    {
        MakeNoise();
        xOffset += Time.deltaTime * timeScale;
        yOffset += Time.deltaTime * timeScale;
    }

    private Mesh GenerateMesh()
    {
        Mesh m = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        for (int x = 0; x <= gridSize; x++)
        {
            for (int y = 0; y <= gridSize; y++)
            {
                vertices.Add(new Vector3(-size * 0.5f + size * (x / ((float)gridSize)), 0, -size * 0.5f + size * (y / ((float)gridSize))));
                normals.Add(Vector3.up);
                uvs.Add(new Vector2(x / (float)gridSize, y / (float)gridSize));
            }
        }
        List<int> triangles = new List<int>();
        int vertCount = gridSize + 1;
        for (int i = 0; i < vertCount * vertCount - vertCount; i++)
        {
            if ((i + 1) % vertCount == 0)
                continue;
            triangles.AddRange(new List<int>(){
                i + 1 + vertCount, i + vertCount, i, i, i + 1, i + vertCount + 1});
        }

        m.SetVertices(vertices);
        m.SetNormals(normals);
        m.SetTriangles(triangles, 0);

        return m;
    }

    private void MakeNoise()
    {
        Vector3[] vertices = mf.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z) * power;
        mf.mesh.vertices = vertices;
    }

    private float CalculateHeight(float x, float y)
    {
        float xCoord = x * scale + xOffset;
        float yCoord = y * scale + yOffset;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}