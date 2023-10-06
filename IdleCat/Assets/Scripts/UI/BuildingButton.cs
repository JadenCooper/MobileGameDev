using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text buildingName;

    public void Initialize(Building building)
    {
        icon.sprite = building.Icon;
        buildingName.text = building.Name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
