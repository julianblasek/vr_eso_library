using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public float speed; // Geschwindigkeit der Kamerafahrt
    private bool isMoving = false; // Steuert, ob die Kamera sich bewegen soll
    private float elapsedTime = 0f; // Eigener Zeitmesser für die Bewegung der Kamera

    public float height;
    public float radius;

    void Start()
    {
        // Setze die Hintergrundfarbe der Kamera auf dunkel
        Camera.main.backgroundColor = Color.black;
    }

    void Update()
    {
        // Überprüfe, ob die Taste "S" gedrückt wird
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Wechsel den Bewegungszustand der Kamera
            isMoving = !isMoving;
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
            radius = radius*0.75f;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            
            radius = radius*1.5f;
        }
        // Aktualisiere elapsedTime nur, wenn sich die Kamera bewegt
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
        }

        // Berechne die Position für die elliptische Bahn basierend auf elapsedTime
        float angle = elapsedTime * speed;
        float x = Mathf.Sin(angle) * radius; // Radius in x-Richtung
        float y = Mathf.Cos(angle/2) * height;
        float z = Mathf.Cos(angle) * radius; // Radius in z-Richtung

        // Setze die Position der Kamera, unabhängig davon, ob sie sich bewegt oder nicht
        transform.position = new Vector3(x, y, -z);

        // Lasse die Kamera um das Zielobjekt rotieren, auch wenn sie stillsteht
        transform.LookAt(target);
    }
}
