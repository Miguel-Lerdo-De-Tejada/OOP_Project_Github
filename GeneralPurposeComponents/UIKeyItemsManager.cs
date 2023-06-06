using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIKeyItemsManager : MonoBehaviour
{
    [SerializeField, Tooltip("List of UI Key Items in this Scene: ")] List<GameObject> uiKeyItems = new List<GameObject>();

    public void SetActive(int keyName, bool isActive) { uiKeyItems[keyName].SetActive(isActive); }
}
