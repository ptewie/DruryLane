using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalController : MonoBehaviour
{
    // Setting these as delegates so I can access these anywhere
    public delegate void UniversalAction();
    public static event UniversalAction OnQuitGame;
    public static event UniversalAction OnTogglePause;
    public static event UniversalAction DebugHazardCheck;
    public static event UniversalAction DebugHazardSort;
    public static event UniversalAction DebugHazardTypeList;

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

        if (Input.GetKeyDown(KeyCode.H))
        {
           DebugHazardCheck?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
           DebugHazardSort?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
           DebugHazardTypeList?.Invoke();
        }
    }
}