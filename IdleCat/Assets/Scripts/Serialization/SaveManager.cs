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
        string newSaveName = Application.persistentDataPath + "/saves/" + SaveName.text + ".save";
        for (int i = 0; i < SaveFiles.Length; i++)
        {
            // Check For Overwrite
            if (newSaveName == SaveFiles[i])
            {
                // Save With Name Already Present
                UIManager.Instance.ModelWindow.ShowAsDialog("CAUTION", "A Save File With That Name Already Exists. Would You Like To Overwrite It?", "Confirm",
                    "Cancel", null, OnOverwrite, UIManager.Instance.ModelWindow.Close);
                return;
            }
        }
        SerializationManager.Save(SaveName.text, SaveData.current);
    }

    public void OnOverwrite()
    {
        // Delete Present Save Then Create New Save File
        string path = Application.persistentDataPath + "/saves/" + SaveName.text + ".save";
        OnDelete(path);
        SerializationManager.Save(SaveName.text, SaveData.current);
        ShowLoadScreen();
    }

    public void OnDelete(string saveName)
    {
        SerializationManager.delete(saveName);
        ShowLoadScreen();
    }

    public void OnLoad(string saveName)
    {
        SaveData.current = (SaveData)SerializationManager.load(saveName);
        LoadCall?.Invoke();
        VillagerManager.Instance.TriggerNavigation();
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
            int currentIndex = i;
            // Gotta Add Way To Delete Save Too
            GameObject buttonObject = Instantiate(LoadButtonPrefab);
            buttonObject.transform.SetParent(LoadArea, false);

            buttonObject.GetComponent<Button>().onClick.AddListener(() => OnLoad(SaveFiles[currentIndex]));
            Button deleteButton = buttonObject.GetComponentsInChildren<Button>()[1];
            deleteButton.onClick.AddListener(() => OnDelete(SaveFiles[currentIndex]));

            buttonObject.GetComponentInChildren<TextMeshProUGUI>().text = SaveFiles[i].Replace(Application.persistentDataPath + "/saves/", "");
        }
    }
}
