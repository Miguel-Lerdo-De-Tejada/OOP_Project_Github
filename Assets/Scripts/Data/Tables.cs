using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusTable
{
    // attributes.
    // Gamer information
    public string gamerName;
    public int enemiesKilled;

    // General information:
    public int id;
    public string actorName;
    public string description;    
    
    // Level & experience:
    public int level;
    public int maxLevel;
    public int experienceNextLevel;
    public int experienceGrouthRate;
    public int experience;

    // Life & mana attributes:
    public int max_health;
    public int resurrections;    
    public int health;
    public int max_mana;    
    public int mana;
    
    // Basic Atributes:
    public int strength;
    public int endurance;
    public int dexterity;
    public int wisdom;
    public int faith;
    public int resistance;


    // Bad Stats:
    public bool death;
    public bool sleep;
    public bool poison;
    public bool hungry;
    public bool blood;
    public bool burn;
    public bool slow;
    public bool stop;
    public bool confuse;
    public bool berserk;
    public bool posess;
    
    // Good Stats:
    public bool protect;
    public bool resist;
    public bool reflect;
    public bool bost;
    public bool magicBost;
    public bool quick;

    // Reward & Alignment:
    public int experienceReward;
    public bool goodAlignment;

    // Is player activated by a Teleport door:
    public bool isTeleported;
    public bool isTeleportingActivated;

    // Player position:
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 cameraPosition;
    public Quaternion cameraRotation;
}

[System.Serializable]
public class ScenesTable
{    
    // Active Scene:
    public int index;
    public List<Room> room;
    public List<NPC> npcs;
    public List<Computer> computers;
    public int countingMissionCurrentCounting;
    public List<bool> keyItemsList;
    public double timeLineTime;
    public List<Key> doorKeysHoldingList;
    public List<DoorKeyFile> doorKeysList;
    public List<DoorLockFile> doorsLockList;
}

[System.Serializable]
public class Room
{
    // Room states:
    public string name;
    public bool state;
}

[System.Serializable]
public class Computer
{
    public string name;
    public bool isKeyOffered;
    public int turns;
}


[System.Serializable]
public class NPC
{
    // NPCs states:    
    public StatusTable statusTable = new StatusTable();
    public bool counted;
    public bool showGift;
    public bool isKeyFindedMissionRewardApplyed;
    public bool isCountingMissionRewardApplyed;
    public int patrolPoint;
}

[System.Serializable]
public class DoorKeyFile
{
    public Color color;
    public bool isObtained;
}

[System.Serializable]
public class DoorLockFile
{
    public string name;
    public bool removeOnUse;
    public bool isOpened;
    public bool isKeyObtained;    
}

[System.Serializable]
public class EnemiesKilled
{
    public int killed;
    public string gamerName;
}

[System.Serializable]
public class GamerEnemiesKilledTable
{
    public List<EnemiesKilled> gamerkilledList;
}

[System.Serializable]
public class BestExperience
{
    public int level;
    public int experience;
    public string gamerName;
}

[System.Serializable]
public class GamerBestExperienceTable
{
    public List<BestExperience> gamerExperienceList;
}
