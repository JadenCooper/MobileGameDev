using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [Header("Notebook Settings")]
    public GameObject Notebook;
    public TweenMove NotebookMove;
    public Vector3 OpenNotebook;
    public Vector3 ClosedNotebook;

    [Header("Pause Menu Settings")]
    public GameObject PauseMenu;
    public TweenMove PauseMenuMove;
    public Vector3 OpenPauseMenu;
    public Vector3 ClosedPauseMenu;
    public void AlterNotebookState()
    {
        TogglePause();

        if (Notebook.activeSelf)
        {
            // Already Open So Close
            NotebookMove.Move(ClosedNotebook, ChangeNotebookState);
        }
        else
        {
            ChangeNotebookState();
            NotebookMove.Move(OpenNotebook);
        }
    }
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

    public void ChangeNotebookState()
    {
        Notebook.SetActive(!Notebook.activeSelf);
    }
    public void AlterPauseMenuState()
    {
        TogglePause();

        if (PauseMenu.activeSelf)
        {
            // Already Open So Close
            PauseMenuMove.Move(ClosedPauseMenu, ChangePauseMenuState);
        }
        else
        {
            ChangePauseMenuState();
            PauseMenuMove.Move(OpenPauseMenu);
        }
    }

    public void ChangePauseMenuState()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
    }

}
