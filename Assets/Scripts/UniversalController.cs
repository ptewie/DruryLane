using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalController : MonoBehaviour
{
    // Setting these as delegates so I can access these anywhere
    public delegate void UniversalAction();
    public static event UniversalAction OnQuitGame;
    public static event UniversalAction OnTogglePause;

    private void Update()
    {
        // Constantly check for inputs every frame
        CheckForInput();
    }

    private void CheckForInput()
    {
        // check for quit input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnQuitGame?.Invoke();
        }

        // check for pause input
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnTogglePause?.Invoke();
        }
    }
}