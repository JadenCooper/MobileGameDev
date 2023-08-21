using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConversation", menuName = "Data/Conversation")]
public class Conversation : ScriptableObject
{
    public List<Dialogue> dialogues = new List<Dialogue>();
}
