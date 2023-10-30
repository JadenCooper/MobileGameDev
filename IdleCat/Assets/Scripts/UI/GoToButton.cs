using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoToButton : MonoBehaviour, IPointerClickHandler
{
    public Vector2 location;
    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.MoveCamera(location);
    }
}
