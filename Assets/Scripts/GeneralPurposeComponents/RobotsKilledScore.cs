using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RobotsKilledScore : MonoBehaviour
{
    [SerializeField, Tooltip("UIRecord of robots killed.")]TextMeshProUGUI uiTxtRecordRobotsKilled;
    [SerializeField,Tooltip("UI Current gamer robots killed.")]TextMeshProUGUI uiTxtGamerRobotsKilled;
    HallOfHonor hallOfHonor;

    // Start is called before the first frame update
    void Start()
    {
        hallOfHonor = GetComponent<HallOfHonor>();
        SetUIText();
    }

    // Update is called once per frame
    void Update()
    {
        SetUIText();
    }

    void SetUIText()
    {
        GameObject player = Sensor.GetNearbyGameObjects(Layers.player, transform.position, Mathf.Infinity)[0];
        Status playerStatus = player.GetComponent<Status>();
        int bestEnemiesKilled = hallOfHonor.ReadBestGamerRobotsKiller().killed;
                
        if (bestEnemiesKilled >= playerStatus.enemiesKilled)
        {
            uiTxtRecordRobotsKilled.text = $"Best record name: {hallOfHonor.ReadBestGamerRobotsKiller().gamerName}, Robots killed: {bestEnemiesKilled}.";
        }
        else
        {
            uiTxtRecordRobotsKilled.text = $"Best record name: {playerStatus.gamerName}, Robots killed: {playerStatus.enemiesKilled}.";                
        }

        uiTxtGamerRobotsKilled.text = $"Gamer name: {playerStatus.gamerName}, Robots killed: {playerStatus.enemiesKilled}.";
    }
}
