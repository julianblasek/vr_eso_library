using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public float speed; // Geschwindigkeit der Kamerabewegung
    public Transform cameraRig; // Referenz zum CameraRig

    private Vector3 initialPosition; // Initiale Position des CameraRig
    private Vector3 initialRotation; // Initiale Rotation des CameraRig

    void Start()
    {
        if (cameraRig == null)
        {
            Debug.LogError("CameraRig nicht gefunden. Stelle sicher, dass es in der Szene vorhanden und richtig benannt ist.");
        }
        else
        {
            // Initiale Position und Rotation auslesen und speichern
            initialPosition = cameraRig.position;
            initialRotation = cameraRig.rotation.eulerAngles;
        }
    }

    void Update()
    {
        // Berechne die Bewegungsrichtung basierend auf Tastatureingaben
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // "A" und "D" Tasten
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime; // "W" und "S" Tasten

        // Aktualisiere die Position des CameraRig basierend auf der Eingabe
        if (cameraRig != null)
        {
            cameraRig.Translate(horizontal, 0, vertical);
            cameraRig.LookAt(target);
        }

        // Beispiel: Position und Rotation im Update ändern
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        if (cameraRig != null)
        {
            cameraRig.position = initialPosition;
            cameraRig.rotation = Quaternion.Euler(initialRotation);
        }
    }
}