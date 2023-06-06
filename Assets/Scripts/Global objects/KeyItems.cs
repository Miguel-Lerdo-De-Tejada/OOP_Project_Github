using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyItems
{    
    public static List<bool> keyItemsList = new List<bool>();

    public static bool IsLoaded { get { return _isLoaded; } set { } }

    private static bool _isLoaded;

    public static void PickUp(int keyItemName) { keyItemsList[keyItemName] = true; }

    public static void Drop(int keyItemName) { keyItemsList[keyItemName] = false; }

    public static void AddKeyItems() { for (int i = 0; i < (int)KeyItemName.count; i++) { keyItemsList.Add(false); } }

    public static void ClearKeyItems() { keyItemsList.Clear(); }

    public static void LoadKeyItems (List<bool> keyItemsTable)
    {
        ClearKeyItems();
        keyItemsList.AddRange(keyItemsTable);
        _isLoaded = true;
    }

    public static List<bool> ReadKeyItems() { return keyItemsList; }

    public static void RestartKeyItems() 
    { 
        ClearKeyItems();
        AddKeyItems();
    }
}