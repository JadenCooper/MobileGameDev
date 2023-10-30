using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ModelWindowPanel : MonoBehaviour
{
    [Header("Header")]
    [SerializeField]
    private Transform headerArea;
    [SerializeField]
    private TextMeshProUGUI titleField;

    [Header("Content")]
    [SerializeField]
    private Transform contentArea;
    [SerializeField]
    private Transform verticalLayoutArea;
    [SerializeField]
    private Image HeroImage;
    [SerializeField]
    private TextMeshProUGUI heroText;
    [Space()]
    [SerializeField]
    private Transform horizontalLayoutArea;
    [SerializeField]
    private Transform iconContainer;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TextMeshProUGUI iconText;

    [Header("Footer")]
    [SerializeField]
    private Transform footerArea;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private TextMeshProUGUI confirmText;
    [SerializeField]
    private Button alternateButton;
    [SerializeField]
    private TextMeshProUGUI alternateText;
    [SerializeField]
    private Button declineButton;
    [SerializeField]
    private TextMeshProUGUI declineText;


    private Action onConfirmAction;
    private Action onAlternateAction;
    private Action onDeclineAction;

    [SerializeField]
    private TweenScale tweenScale;
    public void ShowAsHero(string title, Sprite imageToShow, string message, string confirmMessage, string declineMessage, string alternateMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {

        horizontalLayoutArea.gameObject.SetActive(false);
        verticalLayoutArea.gameObject.SetActive(true);

        // Hide header if there's no title
        bool hasTitle = string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(!hasTitle);
        titleField.text = title;

        HeroImage.sprite = imageToShow;
        heroText.text = message;

        onConfirmAction = confirmAction;
        confirmText.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        declineButton.gameObject.SetActive(hasDecline);
        declineText.text = declineMessage;
        onDeclineAction = declineAction;
        
        bool hasAlternate = (alternateAction != null);
        alternateButton.gameObject.SetActive(hasAlternate);
        alternateText.text = alternateMessage;
        onAlternateAction = alternateAction;

        Show();
    }

    public void ShowAsHero(string title, Sprite imageToShow, string message, Action confirmAction)
    {
        ShowAsHero(title, imageToShow, message, "Continue", "", "", confirmAction);
    }
    public void ShowAsHero(string title, Sprite imageToShow, string message, Action confirmAction, Action declineAction)
    {
        ShowAsHero(title, imageToShow, message, "Continue", "Back", "", confirmAction, declineAction);
    }

    public void ShowAsPrompt(string title, Sprite imageToShow, string message, string confirmMessage, string declineMessage, string alternateMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        horizontalLayoutArea.gameObject.SetActive(true);
        verticalLayoutArea.gameObject.SetActive(false);

        // Hide header if there's no title
        bool hasTitle = string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(!hasTitle);
        titleField.text = title;

        iconImage.sprite = imageToShow;
        iconText.text = message;

        onConfirmAction = confirmAction;
        confirmText.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        declineButton.gameObject.SetActive(hasDecline);
        declineText.text = declineMessage;
        onDeclineAction = declineAction;

        bool hasAlternate = (alternateAction != null);
        alternateButton.gameObject.SetActive(hasAlternate);
        alternateText.text = alternateMessage;
        onAlternateAction = alternateAction;

        Show();
    }
    public void ShowAsPrompt(string title, Sprite imageToShow, string message, Action confirmAction)
    {
        ShowAsPrompt(title, imageToShow, message, "Continue", "", "", confirmAction);
    }
    public void ShowAsPrompt(string title, Sprite imageToShow, string message, Action confirmAction, Action declineAction)
    {
        ShowAsPrompt(title, imageToShow, message, "Continue", "Back", "", confirmAction, declineAction);
    }

    public void ShowAsDialog(string title, string message, string confirmMessage, string declineMessage, string alternateMessage, Action confirmAction, Action declineAction = null, Action alternateAction = null)
    {
        horizontalLayoutArea.gameObject.SetActive(true);
        iconImage.gameObject.SetActive(false);
        verticalLayoutArea.gameObject.SetActive(false);

        // Hide header if there's no title
        bool hasTitle = string.IsNullOrEmpty(title);
        headerArea.gameObject.SetActive(!hasTitle);
        titleField.text = title;

        iconText.text = message;

        onConfirmAction = confirmAction;
        confirmText.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        declineButton.gameObject.SetActive(hasDecline);
        declineText.text = declineMessage;
        onDeclineAction = declineAction;

        bool hasAlternate = (alternateAction != null);
        alternateButton.gameObject.SetActive(hasAlternate);
        alternateText.text = alternateMessage;
        onAlternateAction = alternateAction;

        Show();
    }
    public void Confirm()
    {
        onConfirmAction?.Invoke();
        Close();
    }

    public void Alternate()
    {
        onAlternateAction?.Invoke();
        Close();
    }

    public void Decline()
    {
        onDeclineAction?.Invoke();
        Close();
    }

    public void Close()
    {
        confirmButton.onClick.RemoveAllListeners();
        alternateButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();

        tweenScale.ScaleDown(null);
    }

    public void Show()
    {
        confirmButton.onClick.AddListener(Confirm);
        alternateButton.onClick.AddListener(Alternate);
        declineButton.onClick.AddListener(Decline);

        tweenScale.ScaleUp(null);
    }
}
