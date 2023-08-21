using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<Dialogue> ConversationDialogues = new Queue<Dialogue>();
    private Queue<string> sentences = new Queue<string>();
    public TMP_Text NameText;
    public TMP_Text DialogueText;
    private Animator animator;
    public static DialogueManager Instance { get; private set; }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void StartConversation(Conversation conversation)
    {
        animator.SetBool("IsOpen", true);

        ConversationDialogues.Clear();
        foreach (Dialogue dialogue in conversation.dialogues)
        {
            ConversationDialogues.Enqueue(dialogue);
        }

        StartDialogue(ConversationDialogues.Dequeue());
    }

    public void StartDialogue(Dialogue dialogue)
    {
        NameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(); // Make Sure First Sentence Is Present
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            // If No More Sentences End Dialogue

            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        DialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Could Be Changed In The Future To Suit Who's Speaking EG They Person's Cadence
        }
    }

    public void EndDialogue()
    {
        if (ConversationDialogues.Count == 0)
        {
            // End Conversation
            animator.SetBool("IsOpen", false);
        }
        else
        {
            StartDialogue(ConversationDialogues.Dequeue());
        }
    }
}
