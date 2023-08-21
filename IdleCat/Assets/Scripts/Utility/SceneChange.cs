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
                SceneManager.LoadScene("PlayMode");
                break;

            default:
                Debug.Log("Scene Change Index Entered Incorrectly");
                break;
        }
    }
}
