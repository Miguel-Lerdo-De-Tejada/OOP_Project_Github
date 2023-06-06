using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupKeyItem : Pickup
{    
    [SerializeField, Tooltip("KeyItemName")] KeyItemName keyName;

    protected override void OnAction()
    {
        if (IsKeyItemsListEmpty()) 
        {
            AddKeyItems();            
        }
        Pickup();
    }

    bool IsKeyItemsListEmpty() { return KeyItems.keyItemsList.Count <= 0; }

    void AddKeyItems()
    {
        KeyItems.AddKeyItems();
    }

    void Pickup()
    {
        if (!KeyItems.keyItemsList[(int)keyName])
        {
            UIKeyItemsManager keyItems = GameObject.FindObjectOfType<UIKeyItemsManager>();

            KeyItems.PickUp((int)keyName);
            keyItems.SetActive((int)keyName, true);
        }
    }

    public int ReadKeyNameValue() 
    {
        return (int) keyName; 
    }
}
