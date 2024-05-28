using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;

    void Start()
    {
        // Stelle sicher, dass die Hauptkamera beim Start aktiv ist
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        secondaryCamera.backgroundColor = Color.black;
        mainCamera.backgroundColor = Color.black;

    }

    void Update()
    {
        // Überprüfe, ob die Umschalttaste (z.B. 'C') gedrückt wird
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Schalte zwischen den Kameras um
            mainCamera.enabled = !mainCamera.enabled;
            secondaryCamera.enabled = !secondaryCamera.enabled;
            mainCamera.backgroundColor = Color.black;
            secondaryCamera.backgroundColor = Color.black;
        }

        

    }


}
