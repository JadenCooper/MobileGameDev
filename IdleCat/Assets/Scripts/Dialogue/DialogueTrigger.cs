using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Conversation conversation;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartConversation(conversation);
    }
}
