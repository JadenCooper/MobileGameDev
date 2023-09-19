using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image  background;
    public TMP_Text text;
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    private void Start()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TMP_Text>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClickC");
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ClickEt");
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ClickEx");
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }

    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }

}
