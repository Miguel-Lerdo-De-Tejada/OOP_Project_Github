using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Message",menuName ="Message")]
public class Message : ScriptableObject
{
    [Tooltip("Computer name when player interacts with it.")]
    public string computerName;
    [Tooltip("Computer message when player interacts with it."),TextArea(4,3)]
    public List<string> computerMessages = new List<string>();
}
