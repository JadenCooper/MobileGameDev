using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBuildingButton : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text name;
    private Building building;
    public void Initialize(Building building)
    {
        this.building = building;
        icon.sprite = building.Icon;
        name.text = building.Name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Open Building Display \\
    }
}
