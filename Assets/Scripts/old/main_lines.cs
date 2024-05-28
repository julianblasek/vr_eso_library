using UnityEngine;
using System.Collections.Generic;

public class DataVisualization3 : MonoBehaviour
{
    public TextAsset dataFile;
    public Material pointMaterial;
    public Material lineMaterial;  // Material für das Linien-Mesh

    private Mesh sphereMesh;
    private Mesh lineMesh;
    private List<Matrix4x4> matrices = new List<Matrix4x4>();
    private List<Vector3> velocities = new List<Vector3>();
    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> originalPositions = new List<Vector3>(); // Zum Speichern der ursprünglichen Positionen
    private MaterialPropertyBlock props;
    private float factor= 308.6f;
    private bool isMoving = true;  // Steuerung der Bewegung

    void Start()
    {
        Application.targetFrameRate = 90;
        string[] dataLines = dataFile.text.Split('\n');

        sphereMesh = CreateSphereMesh();
        lineMesh = new Mesh();
        props = new MaterialPropertyBlock();

        for (int i = 1; i < dataLines.Length - 1; i++)
        {
            string[] data = dataLines[i].Split(',');

            if (data.Length >= 9)
            {
                if (TryParseFloat(data[2], out float x) &&
                    TryParseFloat(data[4], out float y) &&
                    TryParseFloat(data[3], out float z) &&
                    TryParseFloat(data[5], out float v) &&
                    v >= 200)
                {
                    Vector3 position = new Vector3(x, y, z) * 20;
                    matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * 0.5f));
                    velocities.Add(position.normalized * v / factor);
                    positions.Add(position);
                    originalPositions.Add(position); // Speichern der ursprünglichen Position

                    Color color = new Color(float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]), 1.0f);
                    props.SetColor("_EmissionColor", color);
                }
            }
            else
            {
                Debug.LogWarning($"Skipping incomplete data line at index {i + 1}.");
            }
        }

        UpdateLineMesh();  // Initialer Aufruf, um das Linien-Mesh zu erstellen
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isMoving = !isMoving;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < originalPositions.Count; i++)
            {
                positions[i] = originalPositions[i];
                matrices[i] = Matrix4x4.TRS(originalPositions[i], Quaternion.identity, Vector3.one * 0.1f);
            }
        }

        if (isMoving)
        {
            for (int i = 0; i < matrices.Count; i++)
            {
                Vector3 position = positions[i] + velocities[i] * Time.deltaTime;
                positions[i] = position;
                matrices[i] = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * 0.1f);
            }
        }

        UpdateLineMesh();  // Aktualisieren des Linien-Meshes basierend auf neuen Positionen

        if (matrices.Count > 0)
        {
            Graphics.DrawMeshInstanced(sphereMesh, 0, pointMaterial, matrices, props);
        }

        // Zeichnen des Linien-Meshes
        if (lineMesh != null)
        {
            Graphics.DrawMesh(lineMesh, Vector3.zero, Quaternion.identity, lineMaterial, 0);
        }
    }

    private void UpdateLineMesh()
    {
        if (positions.Count < 2)
            return;

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();

        float maxDistance = 0.75f; // Maximaler Abstand, um eine Linie zu zeichnen

        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i + 1; j < positions.Count; j++)
            {
                if (Vector3.Distance(positions[i], positions[j]) <= maxDistance)
                {
                    int startIndex = vertices.Count;
                    vertices.Add(positions[i]);
                    vertices.Add(positions[j]);

                    indices.Add(startIndex);
                    indices.Add(startIndex + 1);
                }
            }
        }

        lineMesh.Clear();
        lineMesh.vertices = vertices.ToArray();
        lineMesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
    }

    private Mesh CreateSphereMesh()
    {
        return GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshFilter>().sharedMesh;
    }

    bool TryParseFloat(string str, out float result)
    {
        return float.TryParse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result) && !float.IsNaN(result);
    }
}
