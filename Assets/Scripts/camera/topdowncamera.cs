using UnityEngine;

public class CameraController1 : MonoBehaviour
{
    // public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public float speed; // Geschwindigkeit der Kamerafahrt
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public Camera secondaryCamera;
    public float height;

    void Start()
    {
        // Setze die Hintergrundfarbe der Kamera auf dunkel
        secondaryCamera.backgroundColor = Color.black;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    void Update()
    {
        // transform.LookAt(target);
        // Bewege die Kamera in einer geraden Linie über dem Ursprung
        float y = Time.time * speed + height;

        
        transform.position = new Vector3(0, y, 0);
        transform.LookAt(target);
    
    }
}
