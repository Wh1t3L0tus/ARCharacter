using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTerrain : MonoBehaviour {

    public Material m_terrainMaterial;
    public float m_horizontalStep = 0.1f;
    public float m_verticalStep = 0.1f;
    public int m_width = 255;
    public int m_height = 255;
    public float m_maxHeight = 0.5f;
    public float m_minHeight = -0.5f;
    public float m_horizontalFrequency = 1.0f;
    public float m_verticalFrequency = 1.0f;
    public int m_startTriangleCount = 5000;

    private Mesh m_mesh;
    private MeshCollider m_collider;

    private Vector3[] m_vertices = new Vector3[255 * 255];
    private int[] m_indexes;

    private int m_trianglesCount = 0;


    float GetRandomHeight(float x, float z)
    {
        return Mathf.Lerp(m_minHeight, m_maxHeight, Mathf.PerlinNoise(x * m_verticalFrequency, z * m_horizontalFrequency));
    }

    void InitVertices()
    {
        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                m_vertices[i * m_width + j] = new Vector3(j * m_horizontalStep, GetRandomHeight(j * m_horizontalStep, i * m_verticalStep), i * m_verticalStep);
            }
        }
    }

    void InitCollider()
    {
        m_collider = gameObject.AddComponent<MeshCollider>();
    }

    void GenerateTriangles(int quantity)
    {
        m_indexes = new int[3 * quantity];
        int triangles = 0;
        int index = 0;

        for (int i = 0; i < m_height - 1; i++)
        {
            for (int j = 0; j < m_width - 1; j++)
            {
                m_indexes[index++] = i * m_width + j;
                m_indexes[index++] = (i + 1) * m_width + j;
                m_indexes[index++] = (i + 1) * m_width + j + 1;

                triangles++;
                if (triangles >= quantity)
                {
                    m_trianglesCount = triangles;
                    return;
                }

                m_indexes[index++] = i * m_width + j;
                m_indexes[index++] = (i + 1) * m_width + j + 1;
                m_indexes[index++] = i * m_width + j + 1;

                triangles++;
                if (triangles >= quantity)
                {
                    m_trianglesCount = triangles;
                    return;
                }
            }
        }


    }

    void UpdateMesh()
    {
        m_mesh.Clear();
        m_mesh.vertices = m_vertices;
        m_mesh.triangles = m_indexes;
        m_mesh.RecalculateNormals();
    }

    void Start () {

        m_mesh = gameObject.AddComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshRenderer>().material = m_terrainMaterial;
        InitVertices();
        GenerateTriangles(m_startTriangleCount);
        UpdateMesh();

        InitCollider();
    }

    void Update()
    {
        m_collider.enabled = false;
        AddTriangles(100);
        m_collider.enabled = true;
    }
	
    public void AddTriangles(int quantity)
    {
        GenerateTriangles(m_trianglesCount + quantity);
        UpdateMesh();
    }
}
