using UnityEngine;
using System.Collections.Generic;

public class DataVisualizationHB : MonoBehaviour
{
    public TextAsset dataFile;
    public Material pointMaterial;
    public float v_filter;

    private Mesh sphereMesh;
    private List<Matrix4x4> matrices = new List<Matrix4x4>();
    private List<Vector3> velocities = new List<Vector3>();
    private List<Vector3> originalPositions = new List<Vector3>(); // Speichert die ursprünglichen Positionen
    private float factor= 308.60f;
    private MaterialPropertyBlock props;
    private bool isMoving = false;
    public Color linecolor;
    private bool isVisible =false;

    void Start()
    {
        Application.targetFrameRate = 144;
        string[] dataLines = dataFile.text.Split('\n');

        sphereMesh = CreateSphereMesh();

        for (int i = 1; i < dataLines.Length - 1; i++)
        {
            string[] data = dataLines[i].Split(',');

            if (data.Length >= 9)
            {
                if (TryParseFloat(data[2], out float x) &&
                    TryParseFloat(data[4], out float y) &&
                    TryParseFloat(data[3], out float z) &&
                    TryParseFloat(data[5], out float v) &&
                    v >= v_filter)
                {
                    Vector3 position = new Vector3(x, z, y) * 20;
                    matrices.Add(Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * 0.1f));
                    velocities.Add(position.normalized * v / factor);
                    originalPositions.Add(position); // Speichern der ursprünglichen Position

                    props = new MaterialPropertyBlock();
                    Color color = new Color(float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]), 1.0f);
                    props.SetColor("_Color", linecolor);
                    props.SetColor("_EmissionColor", linecolor);
                }
            }
            else
            {
                Debug.LogWarning($"Skipping incomplete data line at index {i + 1}.");
            }
        }
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
                matrices[i] = Matrix4x4.TRS(originalPositions[i], Quaternion.identity, Vector3.one * 0.1f);
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            isVisible = !isVisible;
        }
        if (isMoving)
        {
            for (int i = 0; i < matrices.Count; i++)
            {
                Matrix4x4 matrix = matrices[i];
                Vector3 position = matrix.GetColumn(3); // Extrahiert die Position als Vector3
                position += velocities[i] * Time.deltaTime; // Korrekte Addition von Vector3 zu Vector3
                matrices[i] = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one * 0.1f); // Erzeugt eine neue Transformationsmatrix
            }
        }


        if (matrices.Count > 0 && isVisible)
        {
            Graphics.DrawMeshInstanced(sphereMesh, 0, pointMaterial, matrices, props);
        }
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
