using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMouseCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor when the Escape key is pressed
        }
        else if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor again when the left mouse button is clicked
        }
    }
}
