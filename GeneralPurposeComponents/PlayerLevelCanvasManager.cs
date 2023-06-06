using UnityEngine;

public class PlayerLevelCanvasManager : MonoBehaviour
{
    [SerializeField,Tooltip("Level window")] LevelWindow levelWindow;
    bool isStatusSetted = false;
    Status status;

    private void Update()
    {
        if (!isStatusSetted)
        {
            isStatusSetted = true;
            SetPlayerStatus();
        }
    }

     public void SetLevelByStatus(Status playerStatus)
    {
        levelWindow.SetLevelByStatus(playerStatus);        
    }

    void SetPlayerStatus()
    {        
        GetComponents();
        if (status)
        {
            InitialiceLevelWindowByStatus();
        }
        void GetComponents() { status = GetComponent<Status>(); }
        void InitialiceLevelWindowByStatus() { levelWindow.SetLevelByStatus(status); }
    }
}
