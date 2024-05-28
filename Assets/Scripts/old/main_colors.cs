using UnityEngine;

public class DataVisualization4 : MonoBehaviour
{
    public TextAsset dataFile;
    public Material pointMaterial;

void Start()
{
    Application.targetFrameRate = 90;
    string[] dataLines = dataFile.text.Split('\n');
    GameObject pointCloud = new GameObject("PointCloud");

    for (int i = 1; i < dataLines.Length - 1; i++)
    {
        string[] data = dataLines[i].Split(',');

        if (data.Length < 9) // Überprüfen Sie, ob genügend Daten vorhanden sind
        {
            Debug.LogWarning($"Skipping incomplete data line at index {i + 1}.");
            continue;
        }

        if (TryParseFloat(data[2], out float x) &&
            TryParseFloat(data[4], out float y) &&
            TryParseFloat(data[3], out float z) &&
            TryParseFloat(data[5], out float v) &&
            v >= 75)
        {
            Color color = new Color(float.Parse(data[6]), float.Parse(data[7]), float.Parse(data[8]), 1.0f);

            GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.position = new Vector3(x, y, z)*10;
            point.transform.parent = pointCloud.transform;
            point.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            Renderer renderer = point.GetComponent<Renderer>();
            Material newMat = new Material(pointMaterial);
            renderer.material = newMat; // Verwende die neue Materialinstanz für den Renderer
            newMat.EnableKeyword("_EMISSION"); // Emissionskeyword aktivieren
            newMat.SetColor("_EmissionColor", color); // Setze die Emissionsfarbe auf das Material

            PointExpansion pointExp = point.AddComponent<PointExpansion>();
            pointExp.velocity = v / 200;
        }
        else
        {
            Debug.LogWarning($"Skipping invalid or NaN data line at index {i + 1}.");
        }
    }
}


bool TryParseFloat(string str, out float result)
{
    // Versucht den String zu parsen. Wenn es fehlschlägt oder der Wert NaN ist, wird false zurückgegeben.
    if (float.TryParse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result) && !float.IsNaN(result))
    {   
        return true;
    }
    else
    {
        result = 0.0f; // Dieser Wert wird ignoriert, da false zurückgegeben wird.
        return false;
    }
}

public class PointExpansion : MonoBehaviour
{
    public float velocity; // Geschwindigkeit des Teilchens

    void Update()
    {
        // Berechne die Verschiebung
        float displacement = velocity * Time.deltaTime;

        // Bewege das Teilchen
        transform.position += transform.position.normalized * displacement;
    }
}

}