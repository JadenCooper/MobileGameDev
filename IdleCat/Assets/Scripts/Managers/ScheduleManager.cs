using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleManager : MonoBehaviour
{
    public List<ScheduleButton> scheduleButtons = new List<ScheduleButton>();

    public Color Anything;
    public Color Work;
    public Color Recreation;
    public Color Rest;
    public TMP_Text FullName;
    public Image VillagerIcon;
    private int CurrentVillagerIndex = 0;
    public VillagerState selectedVillagerState = VillagerState.Anything;
    public void Open(int index)
    {
        selectedVillagerState = VillagerState.Anything;
        CurrentVillagerIndex = index;
        VillagerInfo vi = VillagerManager.Instance.Villagers[CurrentVillagerIndex].villagerInfo;
        FullName.text = vi.FirstName + " " + vi.LastName;
        SetSchedule(vi.schedule);
    }
    public void DefaultOpen()
    {
        selectedVillagerState = VillagerState.Anything;
        CurrentVillagerIndex = 0;
        VillagerInfo vi = VillagerManager.Instance.Villagers[CurrentVillagerIndex].villagerInfo;
        FullName.text = vi.FirstName + " " + vi.LastName;
        VillagerIcon.sprite = vi.Species.sprite;
        SetSchedule(vi.schedule); // Set To First Villager Schedule
    }
    public void SetSchedule(Schedule schedule)
    {
        for (int i = 0; i < scheduleButtons.Count; i++)
        {
            SetButton(scheduleButtons[i], schedule.VillagerStates[i]);
        }
    }

    public void ChangeVillager(bool Forward)
    {
        if (Forward)
        {
            CurrentVillagerIndex++;
        }
        else
        {
            CurrentVillagerIndex--;
        }
        CurrentVillagerIndex = (int)Data.Wrap(CurrentVillagerIndex, VillagerManager.Instance.Villagers.Count);
        VillagerInfo vi = VillagerManager.Instance.Villagers[CurrentVillagerIndex].villagerInfo;
        FullName.text = vi.FirstName + " " + vi.LastName;
        VillagerIcon.sprite = vi.Species.sprite;

        SetSchedule(vi.schedule);
    }

    public void SetButton(ScheduleButton button, VillagerState state)
    {
        switch (state)
        {
            case VillagerState.Home:
                button.background.color = Rest;
                button.VillagerState = VillagerState.Home;
                break;

            case VillagerState.Work:
                button.background.color = Work;
                button.VillagerState = VillagerState.Work;
                break;

            case VillagerState.Recreation:
                button.background.color = Recreation;
                button.VillagerState = VillagerState.Recreation;
                break;

            case VillagerState.Anything:
                button.background.color = Anything;
                button.VillagerState = VillagerState.Anything;
                break;
        }
    }

    public void SetCurrentState(ScheduleButton button)
    {
        SetButton(button, selectedVillagerState);
    }

    public void SelectedVillagerState(int index)
    {
        switch (index)
        {
            case 0:
                selectedVillagerState = VillagerState.Anything;
                break;

            case 1:
                selectedVillagerState = VillagerState.Work;
                break;

            case 2:
                selectedVillagerState = VillagerState.Recreation;
                break;

            case 3:
                selectedVillagerState = VillagerState.Home;
                break;

            default:
                Debug.LogWarning("SelectedVillagerState() Broken: Invalid Index");
                break;
        }
    }

    public void Save()
    {
        VillagerInfo vi = VillagerManager.Instance.Villagers[CurrentVillagerIndex].villagerInfo;
        Schedule schedule = vi.schedule;

        for (int i = 0; i < scheduleButtons.Count; i++)
        {
            schedule.VillagerStates[i] = scheduleButtons[i].VillagerState;
        }

    }
}
