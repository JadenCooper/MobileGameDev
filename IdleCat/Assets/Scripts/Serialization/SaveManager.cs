using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public TMP_InputField SaveName;
    public GameObject LoadButtonPrefab;
    public string[] SaveFiles;
    public GameObject LoadArea;
    public void OnSave()
    {
        SerializationManager.Save(SaveName.text, SaveData.Current);
    }

    public void GetLoadFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves/");
        }

        SaveFiles = Directory.GetFiles(Application.persistentDataPath + "/saves/");
    }
    public void ShowLoadScreen()
    {
        GetLoadFiles();

        foreach (Transform button in LoadArea)
        {
            Destroy(button.gameObject);
        }

        for (int i = 0; i < SaveFiles.Length; i++)
        {
            GameObject buttonObject = Instantiate(LoadButtonPrefab);
            buttonObject.transform.SetParent(LoadArea, false);

            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnLoad(SaveFiles[i]);
            });
        }
    }

    public void OnLoad(string saveName)
    {

    }
}
