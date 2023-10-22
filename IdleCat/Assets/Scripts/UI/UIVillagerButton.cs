using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIVillagerButton : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text name;
    private VillagerInfo VI;

    public void Initialize(VillagerInfo villagerInfo)
    {
        VI = villagerInfo;
        icon.sprite = VI.Species.Sprite;
        name.text = VI.FirstName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.VillagerDisplayWindow.OpenWindow(VI);
    }
}
