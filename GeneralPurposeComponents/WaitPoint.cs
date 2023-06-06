using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPoint : MonoBehaviour
{
    [SerializeField, Tooltip("Minutes to wait"), Range(0.1f, 10.0f)] float waitTime;
    [SerializeField, Tooltip("Look at gameObject")] GameObject lookAtGameObject;

    public float ReadWaitTime() { return waitTime; }
    public GameObject ReadLookAtGameObject() { return lookAtGameObject; }
}
