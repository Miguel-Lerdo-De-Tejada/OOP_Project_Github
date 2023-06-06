using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueComponent : MonoBehaviour
{
    [SerializeField, Tooltip("NPC dialogue spcriptable object.")] 
    DialogueExp dialogue;

    public DialogueExp Read() { return dialogue; }
}
