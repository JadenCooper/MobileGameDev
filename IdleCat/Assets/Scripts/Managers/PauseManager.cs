using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            // Currently Paused
            Time.timeScale = 1.0f;
        }
        else
        {
            // Currently In Play Mode
            Time.timeScale = 0f;
        }
    }
}
