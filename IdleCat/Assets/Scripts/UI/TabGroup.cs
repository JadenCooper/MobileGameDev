using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons = new List<TabButton>();
    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;
    public void Subscribe(TabButton button)
    {
        if (!tabButtons.Contains(button))
        {
            tabButtons.Add(button);
        }
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnIndexTabSelected(int Index)
    {
        TabButton button = tabButtons[Index];
        if (button != null)
        {
            OnTabSelected(button);
        }
        else
        {
            Debug.LogWarning("OnIndexTabSelected Method Broken: Index Not Found");
        }
    }
    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = button;

        selectedTab.Select();

        ResetTabs();
        button.background.color = tabActive;
        button.text.color = tabIdle;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.color = tabIdle;
            button.text.color = tabActive;
        }
    }
}