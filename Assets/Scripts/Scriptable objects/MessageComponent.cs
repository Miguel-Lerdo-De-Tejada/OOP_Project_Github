using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageComponent : MonoBehaviour
{
    [SerializeField, Tooltip("Message component sended in the computer")]
    Message message;

    public Message Read() { return message; }
}
