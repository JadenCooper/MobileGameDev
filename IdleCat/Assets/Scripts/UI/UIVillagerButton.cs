using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIVillagerButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text name;
    public VillagerInfo VI;
    public Button button;
    private Building building;
    public void Initialize(VillagerInfo villagerInfo = null, Sprite givenSprite = null, Building building = null)
    {
        this.building = building;

        if (villagerInfo == null)
        {
            icon.sprite = givenSprite;
            name.text = "Unknown";
            VI = null;
        }
        else
        {
            VI = villagerInfo;
            icon.sprite = VI.Species.Sprite;
            name.text = VI.FirstName;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (VI != null)
        {
            UIManager.Instance.VillagerDisplayWindow.OpenWindow(VI);
        }
    }

    public void assign()
    {
        switch (building.buildingType)
        {
            case BuildingType.Job:
                building.GetComponent<Job>().AssignEmployee(VI);
                break;

            case BuildingType.House:
                building.GetComponent<House>().AssignInhabitant(VI);
                break;

            default:
                break;
        }
    }
}
