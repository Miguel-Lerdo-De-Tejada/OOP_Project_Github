using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField, Tooltip("Set the player light game object here")] GameObject playerLight;
    
    public bool ActiveSelf() { return playerLight.activeSelf; }

    public void SetActive(bool isActive) { playerLight.SetActive(isActive); }
}
