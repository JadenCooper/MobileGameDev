using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScheduleButton : MonoBehaviour, IPointerClickHandler
{
    public Image background;
    public ScheduleManager scheduleManager;
    public VillagerState VillagerState;
    public void OnPointerClick(PointerEventData eventData)
    {
        scheduleManager.SetCurrentState(this);
    }
}
