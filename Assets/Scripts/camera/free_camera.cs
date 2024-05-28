using UnityEngine;
using System;

public class free_camera : MonoBehaviour
{
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    public float speed; // Geschwindigkeit der Kamerabewegung

    void Start()
    {
        // Setze die Hintergrundfarbe der Kamera auf dunkel
        Camera.main.backgroundColor = Color.black;
    }

    void Update()
    {
        // Berechne die Bewegungsrichtung basierend auf Tastatureingaben
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime; // "A" und "D" Tasten
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime; // "W" und "S" Tasten



        // Aktualisiere die Position der Kamera basierend auf der Eingabe
        transform.Translate(horizontal, 0, vertical);


        // Lasse die Kamera weiterhin auf das Zielobjekt blicken
        transform.LookAt(target);
    }
}