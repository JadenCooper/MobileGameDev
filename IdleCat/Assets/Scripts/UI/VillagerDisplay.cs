using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VillagerDisplay : MonoBehaviour
{
    public UIVillagerButton Father;
    public UIVillagerButton Mother;
    public List<UIVillagerButton> Children = new List<UIVillagerButton>();
    public List<TMP_Text> VillagerDetails = new List<TMP_Text>();

    public GameObject UIVillagerButtonPrefab;

    public GameObject ChildrenParent;
    private VillagerInfo currentVI;
    public void OpenWindow(VillagerInfo VI)
    {
        CloseWindow();

        currentVI = VI;

        SetFamilyTree();
        SetVillagerDetails();

        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    private void SetVillagerDetails()
    {
        VillagerDetails[0].text = currentVI.FirstName;
        VillagerDetails[1].text = currentVI.LastName;
        VillagerDetails[2].text = currentVI.Species.Species;
        VillagerDetails[3].text = currentVI.Age.ToString(); 
        VillagerDetails[4].text = currentVI.Sex; 
        
        // Will Eventually Pre Process It Into A Phrase Eg Instead Of Working, Cutting Blocks At The Mason
        VillagerDetails[5].text = currentVI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x].ToString();
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
