using UnityEngine;
using Valve.VR;
using System;
using System.Collections;
using System.Collections.Generic;

public class test3 : MonoBehaviour
{
    public Transform target; // Zielobjekt, auf das die Kamera gerichtet sein soll
    private float speed = 5.0f; // Geschwindigkeit der Kamerabewegung
    public Transform cameraRig; // Referenz zum CameraRig
    private float minDistance = 0.1f;

    private Vector3 initialPosition; // Initiale Position des CameraRig
    private Vector3 initialRotation; // Initiale Rotation des CameraRig

    private float horizontal;
    private float vertical;

    // SteamVR-Action für das Trackpad
    public SteamVR_Action_Vector2 moveAction;

    // SteamVR-Action für die Höhenbewegung
    public SteamVR_Action_Boolean moveUp; 
    public SteamVR_Action_Boolean moveDown;
    public SteamVR_Input_Sources handType = SteamVR_Input_Sources.Any;

    void Start()
    {
        if (cameraRig == null)
        {
            Debug.LogError("CameraRig nicht gefunden. Stelle sicher, dass es in der Szene vorhanden und richtig benannt ist.");
        }
        else
        {
            cameraRig.LookAt(target);
            // Initiale Position und Rotation auslesen und speichern
            initialPosition = cameraRig.position;
            initialRotation = cameraRig.rotation.eulerAngles;

            // Initialisiere die horizontalen und vertikalen Bewegungen mit 0
            horizontal = 0f;
            vertical = 0f;
        }
    }

    void Update()
    {
        if (cameraRig != null)
        {
            // Steuerung über HTC Vive Controller mit SteamVR
            if (moveAction != null)
            {
                Vector2 moveValue = moveAction.axis;

                // Überprüfen, ob das Trackpad berührt wird
                if (moveValue != Vector2.zero)
                {
                    // Verwende die Werte des Trackpads für die Bewegung
                    horizontal = moveValue.x * speed * Time.deltaTime;
                    vertical = moveValue.y * speed * Time.deltaTime * -1f;

                    // Berechne die neue Position
                    Vector3 newPosition = new Vector3(cameraRig.position.x, 0, cameraRig.position.z) + new Vector3(horizontal, 0, vertical);
                    float distanceToTarget = Vector3.Distance(newPosition, target.position);

                    if (distanceToTarget >= minDistance)
                    {
                        // Bewegung nur wenn der Abstand zum Ziel größer als der minimale Abstand ist
                        cameraRig.Translate(horizontal, 0, vertical);
                    }
                }
            }

            // Halte die Rotation konstant unabhängig vom Abstand
            Vector3 directionToTarget = (target.position - cameraRig.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            cameraRig.rotation = Quaternion.Slerp(cameraRig.rotation, targetRotation, Time.deltaTime * speed);

            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetPosition();
            }
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
