using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.FilePathAttribute;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TMP_Text ClockText;
    public List<TMP_Text> ResourceTexts = new List<TMP_Text>(); // Happiness, Wood, Stone, Food, Gold, VillagerCount
    private bool longHourTime = true; // 24 hour time
    private float currentHour = 6;
    public ModelWindowPanel ModelWindow;
    public VillagerDisplay VillagerDisplayWindow;

    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Vector3 playerCameraPosition;

    [SerializeField]
    private GameObject mainUICanvas;
    [SerializeField]
    private GameObject cameraLookCanvas;
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
    public void UpdateClockTime(float hour)
    {
        currentHour = hour;
        UpdateClock();
    }

    private void UpdateClock()
    {
        string backEnd;
        if (!longHourTime)
        {
            // Convert To 12 Hour Time
            if (currentHour > 12)
            {
                currentHour -= 12;
                backEnd = ":00 pm";
            }
            else
            {
                backEnd = ":00 am";
            }
        }
        else
        {
            backEnd = ":00";
        }
        ClockText.text = currentHour.ToString() + backEnd;
    }

    [ContextMenu("Toggle Time Setting")]
    public void ChangeTimeSetting()
    {
        longHourTime = !longHourTime;
        UpdateClock();
    }
    public void UpdateResources(List<float> Resources)
    {
        // Change Resource Displays

    }

    public void MoveCamera(Vector3 location)
    {
        mainUICanvas.SetActive(false);
        cameraLookCanvas.SetActive(true);
        location = transform.TransformPoint(location);
        LeanTween.move(playerCamera.gameObject, location, 0.5f).setIgnoreTimeScale(true).setOnComplete(resetZValue);
    }

    private void resetZValue()
    {
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y, playerCameraPosition.z);
    }
    public void CameraReturn()
    {
        mainUICanvas.SetActive(true);
        cameraLookCanvas.SetActive(false);
        LeanTween.moveLocal(playerCamera.gameObject, playerCameraPosition, 0.5f).setIgnoreTimeScale(true).setOnComplete(resetZValue);
    }
}
