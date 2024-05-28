using UnityEngine;

public class free_background : MonoBehaviour
{
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public float speed; // Geschwindigkeit der Kamerafahrt
    public int radius; // Radius, um die Intensität der Bewegung anzupassen

    void Update()
    {
        // Hole die X- und Z-Positionen der Kamera
        float cameraX = Camera.main.transform.position.x;
        float cameraY = Camera.main.transform.position.y;
        float cameraZ = Camera.main.transform.position.z;

        // Aktualisiere die Position des Hintergrunds in gegenläufiger Richtung
        transform.position = new Vector3(-cameraX * radius, -cameraY * radius, -cameraZ * radius);

        // Lasse das Objekt weiterhin auf das Zielobjekt blicken
        transform.LookAt(target);
    }
}
