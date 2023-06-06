using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue", menuName = "Dialogue")]
public class DialogueExp : ScriptableObject
{
    [Tooltip("Dialogue between player and NPC"),TextArea(4, 8)]
    public List<string> dialogue = new List<string> ();
    [Tooltip("Dialogue turn, npc (1), player (2)"), Range(1, 2)]
    public List<int> turn = new List<int>();
}
