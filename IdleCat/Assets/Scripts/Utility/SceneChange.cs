using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public void ChangeScene(int Scene)
    {
        switch (Scene)
        {
            case 0:
                SceneManager.LoadScene("MainMenu");
                break;

            case 1:
                GameManager.Instance.LoadGame();
                break;

            case 2:
                Application.Quit();
                break;

            default:
                Debug.Log("Scene Change Index Entered Incorrectly");
                break;
        }
    }
}
