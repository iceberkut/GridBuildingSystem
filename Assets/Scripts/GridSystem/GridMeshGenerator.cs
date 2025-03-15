using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMeshGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 1f;
    public Vector2 gridOffset;
    public int gridSize = 10;

    [Header("Visual Settings")]
    public Color gridColor = Color.white;
    public float lineWidth = 0.05f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] indices;


    void Start()
    {
        GenerateGridMesh();
        UpdateMaterial();
    }

    void GenerateGridMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        int verticalLines = gridSize + 1;
        int horizontalLines = gridSize + 1;
        int totalLines = verticalLines + horizontalLines;
        int vertexCount = totalLines * 2;
        int indexCount = totalLines * 2;

        vertices = new Vector3[vertexCount];
        indices = new int[indexCount];

        for (int x = 0; x < verticalLines; x++)
        {
            float xPos = x * cellSize + gridOffset.x;
            int baseIndex = x * 2;

            vertices[baseIndex] = new Vector3(xPos, 0.01f, gridOffset.y);
            vertices[baseIndex + 1] = new Vector3(xPos, 0.01f, gridSize * cellSize + gridOffset.y);
        }

        for (int z = 0; z < horizontalLines; z++)
        {
            float zPos = z * cellSize + gridOffset.y;
            int baseIndex = (verticalLines + z) * 2;

            vertices[baseIndex] = new Vector3(gridOffset.x, 0.01f, zPos);
            vertices[baseIndex + 1] = new Vector3(gridSize * cellSize + gridOffset.x, 0.01f, zPos);
        }

        for (int i = 0; i < indexCount; i++)
        {
            indices[i] = i;
        }

        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        mesh.RecalculateBounds();
    }

    void UpdateMaterial()
    {
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = gridColor;
        GetComponent<MeshRenderer>().material = mat;
    }
    public void UpdateGrid(float newCellSize, Vector2 newOffset, int newSize)
    {
        cellSize = newCellSize;
        gridOffset = newOffset;
        gridSize = newSize;
        GenerateGridMesh();
    }


    void OnValidate()
    {
        if (mesh != null && Application.isEditor)
        {
            GenerateGridMesh();
            UpdateMaterial();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            MeshRenderer gridRenderer = GetComponent<MeshRenderer>();
            gridRenderer.enabled = !gridRenderer.enabled;
        }
    }
}