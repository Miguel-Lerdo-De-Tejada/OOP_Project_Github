using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    public List<GameObject> List { get { return roomList; } private set { value = roomList; } }

    [SerializeField, Tooltip("List of rooms in Scene.")] List<GameObject> roomList;

    List<string> roomNames = new List<string>();
    
    public int RoomIndex(string name) 
    {
        CreateRoomNamesList();        
        return roomNames.IndexOf(name); 
    }

    public void HideRooms()
    {
        if (roomList == null)
        {
            Debug.LogError("No list rooms.");
        }
        else
        {
            foreach (GameObject roomItem in roomList) { roomItem.SetActive(false); }
        }
    }

    public void ActivateRoom (int room)
    {        
        if (room < 0 || room >= roomList.Count) { roomList[0].gameObject.SetActive(true); } else { roomList[room].gameObject.SetActive(true); }        
    }

    void CreateRoomNamesList() 
    { 
        roomNames.Clear();
        foreach (GameObject room in roomList) { roomNames.Add(room.name); } 
    }
}
