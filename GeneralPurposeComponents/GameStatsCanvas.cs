using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatsCanvas : MonoBehaviour
{
    [Header("UI Game stats")]
    [SerializeField, Tooltip("Player name text area")] TextMeshProUGUI txtPlayerName;
    [SerializeField, Tooltip("Player leve")] TextMeshProUGUI txtLevel;
    [SerializeField, Tooltip("Player experience")] TextMeshProUGUI txtExperience;
    [SerializeField, Tooltip("Player resurrects")] TextMeshProUGUI txtResurrections;

    [Space(6)]
    [Header("Data base")]
    [SerializeField, Tooltip("Data base object")] GameObject dataBaseObject;
    Database database;

    [HideInInspector] public string playerName;
    [HideInInspector] public string playerLevel;
    [HideInInspector] public string playerExperience;
    [HideInInspector] public string playerResurrections;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGameStatsUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActualizeGameStats()
    {
        playerName = txtPlayerName.text;
        playerLevel = txtLevel.text;
        playerExperience = txtExperience.text;
        playerResurrections = txtResurrections.text;
    }

    public void UpdateGameStatsUI()
    {
        txtPlayerName.text = playerName;
        txtLevel.text = playerLevel;
        txtExperience.text = playerExperience;
        txtResurrections.text = playerResurrections;
    }
}
