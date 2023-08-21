using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject PlayModeUI;
    public void TogglePause()
    {
        if (PauseMenu.activeSelf)
        {
            // Currently Paused
            Time.timeScale = 1.0f;
            PauseMenu.SetActive(false);
            PlayModeUI.SetActive(true);
        }
        else
        {
            // Currently In Play Mode
            Time.timeScale = 0f;
            PauseMenu.SetActive(true);
            PlayModeUI.SetActive(false);
        }
    }
}
