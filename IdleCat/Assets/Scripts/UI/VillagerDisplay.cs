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
    public void OpenWindow(VillagerInfo VI)
    {
        CloseWindow();
        Time.timeScale = 0f;
        currentVI = VI;

        SetFamilyTree();
        SetVillagerDetails();

        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
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
        Time.timeScale = 1f;
    }

    private void SetVillagerDetails()
    {
        VillagerDetails[0].text = currentVI.FirstName;
        VillagerDetails[1].text = currentVI.LastName;
        VillagerDetails[2].text = currentVI.Species.Species;
        VillagerDetails[3].text = currentVI.LifeStage.ToString(); 
        VillagerDetails[4].text = currentVI.Sex;

        VillagerIcon.sprite = currentVI.Species.Sprite;

        // Will Eventually Pre Process It Into A Phrase Eg Instead Of Working, Cutting Blocks At The Mason
        VillagerDetails[5].text = currentVI.currentState.ToString();
        //VillagerDetails[5].text = currentVI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x].ToString();
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
