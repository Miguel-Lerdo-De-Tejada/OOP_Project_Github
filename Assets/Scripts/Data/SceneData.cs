using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneData : MonoBehaviour
{
    // Game:
    bool isExitingGame = false;

    // Persistance:
    [HideInInspector] public static SceneData instance;
    [HideInInspector] public bool isLoading;
    [HideInInspector] public bool isNewGame;

    // Identity:    
    int id;
    [HideInInspector] public string gamerName;
    [HideInInspector] public int enemiesKilled;

    // Resurrections:
    [HideInInspector] public int resurrections;
    [HideInInspector] public bool isResurrection;

    // Level:
    [HideInInspector] public int level;
    int maxLevel;
    int experienceNextLevel;
    int experienceGrouthRate;
    [HideInInspector] public int experience;

    // Life & mana attributes:
    int max_health;
    int health;
    int max_mana;
    int mana;

    // Basic Atributes:
    int strength;
    int endurance;
    int dexterity;
    int wisdom;
    int faith;
    int resistance;

    // Rewards added.    
    bool isRewardAdded;

    void Awake()
    {        
        InstantiateTable();
    }

    void InstantiateTable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ObtainGamerName(TextMeshProUGUI txtGamerName)
    {
        if (txtGamerName.text.Trim() != string.Empty) { gamerName = txtGamerName.text.Trim(); }
    }

    public void UpdatePlayerData(Status playerStatus)
    {
        // Identity:
        if (id > 0) { playerStatus.id = id; }
        if (!(gamerName.Trim() == string.Empty)) { playerStatus.gamerName = gamerName.Trim(); }
        playerStatus.enemiesKilled = enemiesKilled;

        // Level:
        if (level > 0) { playerStatus.level = level; }
        if (maxLevel > 0) { playerStatus.maxLevel = maxLevel; }
        if (experienceNextLevel > 0) { playerStatus.experienceNextLevel = experienceNextLevel; }
        if (experienceGrouthRate > 0) { playerStatus.experienceGrouthRate = experienceGrouthRate; }
        if (experience > 0) { playerStatus.experience = experience; }

        // Life & mana attributes:
        if (max_health > 0) { playerStatus.max_health = max_health; }
        if (health > 0) { playerStatus.health = health; }
        if (instance.max_mana > 0) { playerStatus.max_mana = max_mana; }
        if (instance.mana > 0) { playerStatus.mana = mana; }

        // Basic Atributes:
        if (strength > 0) { playerStatus.strength = strength; }
        if (endurance > 0) { playerStatus.endurance = endurance; }
        if (dexterity > 0) { playerStatus.dexterity = dexterity; }
        if (wisdom > 0) { playerStatus.wisdom = wisdom; }
        if (faith > 0) { playerStatus.faith = faith; }
        if (resistance > 0) { playerStatus.resistance = resistance; }
    }

    public void UploadPlayerData(Status playerStatus)
    {
        // Identity:
        id = playerStatus.id;
        gamerName = playerStatus.gamerName;
        enemiesKilled = playerStatus.enemiesKilled;

        // Level:
        level = playerStatus.level;
        maxLevel = playerStatus.maxLevel;
        experienceNextLevel = playerStatus.experienceNextLevel;
        experienceGrouthRate = playerStatus.experienceGrouthRate;
        experience = playerStatus.experience;

        // Life & mana attributes:
        max_health = playerStatus.max_health;
        health = playerStatus.health;
        instance.max_mana = playerStatus.max_mana;
        mana = playerStatus.mana;

        // Basic Atributes:
        strength = playerStatus.strength;
        endurance = playerStatus.endurance;
        dexterity = playerStatus.dexterity;
        wisdom = playerStatus.wisdom;
        faith = playerStatus.faith;
        resistance = playerStatus.resistance;
    }

    public bool UpdateCountingMissionRewardAdded()
    {
        return isRewardAdded;
    }

    public void UploadCountingMissionRewardAdded(bool uploadedIsRewardAdded)
    {        
        isRewardAdded = uploadedIsRewardAdded;
    }

    public void SetExitingGame(bool isExiting) { isExitingGame = isExiting; }

    public bool ReadExitingGame() { return isExitingGame; }
}
