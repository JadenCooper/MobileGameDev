using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillagerDisplay : MonoBehaviour
{
    public UIVillagerButton Father;
    public UIVillagerButton Mother;
    public UIVillagerButton Partner;
    public List<UIVillagerButton> Children = new List<UIVillagerButton>();
    public List<TMP_Text> VillagerDetails = new List<TMP_Text>();

    public GameObject UIVillagerButtonPrefab;

    public GameObject ChildrenParent;
    public Image VillagerIcon; 
    private VillagerInfo currentVI;

    [SerializeField]
    private Sprite unknownSprite;
    [SerializeField]
    private TweenScale tweenScale;

    private bool alreadyInMenu = false;
    [SerializeField]
    private Vector3 openScale;

    [SerializeField]
    private GoToButton goToVillager;
    [SerializeField]
    private GoToButton goToJob;
    [SerializeField]
    private GoToButton goToHouse;
    public void OpenWindow(VillagerInfo VI)
    {
        if (Time.timeScale == 0)
        {
            alreadyInMenu = true;
        }
        else
        {
            alreadyInMenu = false;
        }

        CloseWindow();
        Time.timeScale = 0f;
        currentVI = VI;
    }

    public void CloseWindow()
    {
        Father.Initialize(null, unknownSprite);
        Mother.Initialize(null, unknownSprite);

        Partner.gameObject.SetActive(false);

        if (Children.Count > 0)
        {
            foreach (UIVillagerButton child in Children)
            {
                Destroy(child.gameObject);
            }

            Children.Clear();
        }

        if (!alreadyInMenu)
        {
            Time.timeScale = 1f;
        }

        tweenScale.ScaleDown(null, SetVillagerScreen);
    }

    private void SetVillagerScreen()
    {
        SetFamilyTree();
        SetVillagerDetails();
        tweenScale.ScaleUp(openScale);
    }
    private void SetVillagerDetails()
    {
        VillagerDetails[0].text = "First Name: " + currentVI.FirstName;
        VillagerDetails[1].text = "Last Name: " + currentVI.LastName;
        VillagerDetails[2].text = "Species: " + currentVI.Species.Species;
        VillagerDetails[3].text = "Age: " + currentVI.LifeStage.ToString(); 
        VillagerDetails[4].text = "Sex: " + currentVI.Sex;

        VillagerIcon.sprite = currentVI.Species.Sprite;

        // Will Eventually Pre Process It Into A Phrase Eg Instead Of Working, Cutting Blocks At The Mason
        VillagerDetails[5].text = "Current Activity: " + currentVI.currentState.ToString();

        string currentAction = "Current Activity: ";

        if (currentVI.currentState == VillagerState.Traveling)
        {
            // Need To Get Location Traveling To
            currentAction += "Traveling To " + currentVI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];
        }
        else
        {
            switch (currentVI.currentState)
            {
                case VillagerState.Home:
                    currentAction += currentVI.house.ActionDescription;
                    break;

                case VillagerState.Work:
                    currentAction += currentVI.job.ActionDescription;
                    break;

                case VillagerState.Recreation:
                    currentAction += currentVI.recreationGoal.ActionDescription;
                    break;

                case VillagerState.Petitioning:
                    currentAction += "Petitioning The Mayor";
                    break;

                default:
                    Debug.Log("Current Activity Description Broken");
                    break;
            }
        }
        VillagerDetails[5].text = currentAction;

        goToVillager.location = currentVI.gameObject.transform.position;
        goToJob.location = currentVI.job.transform.position;
        goToHouse.location = currentVI.house.transform.position;
    }
    private void SetFamilyTree()
    {
        if (currentVI.Father != null)
        {
            Father.gameObject.SetActive(true);
            Father.Initialize(currentVI.Father);
        }

        if (currentVI.Mother != null)
        {
            Mother.gameObject.SetActive(true);
            Mother.Initialize(currentVI.Mother);
        }

        if (currentVI.Partner != null)
        {
            Partner.gameObject.SetActive(true);
            Partner.Initialize(currentVI.Partner);
        }

        if (currentVI.Children.Count > 0)
        {
            for (int i = 0; i < currentVI.Children.Count; i++)
            {
                GameObject newButton = Instantiate(UIVillagerButtonPrefab);
                newButton.transform.parent = ChildrenParent.transform;

                Children.Add(newButton.GetComponent<UIVillagerButton>());
                Children[i].Initialize(currentVI.Children[i]);
            }
        }
    }
}
