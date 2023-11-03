using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public TMP_InputField SaveName;
    public GameObject LoadButtonPrefab;
    public string[] SaveFiles;
    public Transform LoadArea;
    public event Action SaveCall;
    public event Action LoadCall;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    public void OnSave()
    {
        SaveCall?.Invoke();
        SerializationManager.Save(SaveName.text, SaveData.current);
    }
    public void OnLoad(string saveName)
    {
        LoadCall?.Invoke();
        SaveData.current = (SaveData)SerializationManager.load(Application.persistentDataPath + saveName);
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
            // Gotta Add Way To Delete Save Too
            GameObject buttonObject = Instantiate(LoadButtonPrefab);
            buttonObject.transform.SetParent(LoadArea, false);

            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnLoad(SaveFiles[i]);
            });
            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = SaveFiles[i].Replace(Application.persistentDataPath + "/saves/", "");
        }
    }
}
