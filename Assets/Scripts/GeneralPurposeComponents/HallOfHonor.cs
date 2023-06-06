using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class HallOfHonor : MonoBehaviour
{
    [SerializeField, Tooltip("Enemies killed text UI component")] TextMeshProUGUI textUIGamersEnemiesKilledList;
    [SerializeField, Tooltip("Best gamer experienc text UI component")] TextMeshProUGUI textUIGamersExperienceList;
    [SerializeField, Tooltip("Set to true if this component is intended to load in UI the hall of honor lists")] bool isLoadingUI;
    [SerializeField, Tooltip("Hall of honor is running on a training room?")] bool isTrainingRoom;
    GamerEnemiesKilledTable enemiesKilledTable = new GamerEnemiesKilledTable();
    GamerBestExperienceTable bestExperienceTable = new GamerBestExperienceTable();    
    EnemiesKilled gamerEnemiesKilled = new EnemiesKilled();    
    BestExperience gamerExperience = new BestExperience();


    const string constCarrageReturn = "\r\n";

    // Start is called before the first frame update
    void Start()
    {
        if (isLoadingUI)
        {
            LoadHallofHonorInUI();
        }
        else if (!isTrainingRoom)
        {
            if (SceneData.instance.ReadExitingGame()) { SaveHallOfHonor(); }
        }        
    }

    public EnemiesKilled ReadBestGamerRobotsKiller()
    {
        LoadHallOfHonor();
        return enemiesKilledTable.gamerkilledList[0];
    }

    void SaveHallOfHonor()
    {        
        // Add player data to hall of honor lists.
        AddGamerDataToHallOfHonoer();

        // Save gamer enemies killed table.
        string jsonFile = JsonUtility.ToJson(enemiesKilledTable);
        File.WriteAllText(GamerEnemiesKilledTablePath(), jsonFile);

        // Save gamer best expirence table.
        jsonFile = string.Empty;
        jsonFile = JsonUtility.ToJson(bestExperienceTable);
        File.WriteAllText(GamerBestExperiencePath(), jsonFile);
    }

    void LoadHallofHonorInUI()
    {
        LoadHallOfHonor();
        SetGamerDataToHallOfHonerListsUI();
    }

    void AddGamerDataToHallOfHonoer()
    {
        // Load hall of honor data
        LoadHallOfHonor();

        // Add gamer enemies killed to first five enemies killed list.
        gamerEnemiesKilled.gamerName = SceneData.instance.gamerName;
        gamerEnemiesKilled.killed = SceneData.instance.enemiesKilled;

        if ((enemiesKilledTable.gamerkilledList == null)) { enemiesKilledTable.gamerkilledList = new List<EnemiesKilled>(); }
        enemiesKilledTable.gamerkilledList.Add(gamerEnemiesKilled);
        enemiesKilledTable.gamerkilledList.Sort(SortByEnemiesKilled);

        if (enemiesKilledTable.gamerkilledList.Count > 5)
        {
            for (int i = 0; i < enemiesKilledTable.gamerkilledList.Count; i++)
            {
                if (i > 4) { enemiesKilledTable.gamerkilledList.Remove(enemiesKilledTable.gamerkilledList[i]); }
            }
        }

        // Add gamer experience to first five player experience list.
        if ((bestExperienceTable.gamerExperienceList == null)) { bestExperienceTable.gamerExperienceList = new List<BestExperience>(); }
        gamerExperience.gamerName = SceneData.instance.gamerName;
        gamerExperience.level = SceneData.instance.level;
        gamerExperience.experience = SceneData.instance.experience;

        bestExperienceTable.gamerExperienceList.Add(gamerExperience);
        bestExperienceTable.gamerExperienceList.Sort(SortByExperience);

        if (bestExperienceTable.gamerExperienceList.Count > 5)
        {
            for (int i = 0; i < bestExperienceTable.gamerExperienceList.Count; i++)
            {
                if (i > 4) { bestExperienceTable.gamerExperienceList.Remove(bestExperienceTable.gamerExperienceList[i]); }
            }
        }

        int SortByEnemiesKilled(EnemiesKilled enemiesKilledA, EnemiesKilled enemiesKilledB)
        {
            if (enemiesKilledA.killed < enemiesKilledB.killed)
            {
                return 1;
            }
            else if (enemiesKilledA.killed > enemiesKilledB.killed)
            {
                return -1;
            }

            return 0;
        }
        int SortByExperience(BestExperience experienceA, BestExperience experienceB)
        {
            if (experienceA.level < experienceB.level)
            {
                return 1;
            }
            else if (experienceA.level > experienceB.level)
            {
                return -1;
            }

            if (experienceA.experience < experienceB.experience)
            {
                return 1;
            }
            else if (experienceA.experience > experienceB.experience)
            {
                return -1;
            }

            return 0;
        }
    }

    void LoadHallOfHonor()
    {
        // Load gamer enemies killed table.
        string tablePath = GamerEnemiesKilledTablePath();

        if (File.Exists(tablePath))
        {
            string jsonFile = File.ReadAllText(GamerEnemiesKilledTablePath());
            enemiesKilledTable = JsonUtility.FromJson<GamerEnemiesKilledTable>(jsonFile);
        }

        // Load gamer best experience table.
        tablePath = string.Empty;
        tablePath = GamerBestExperiencePath();

        if (File.Exists(tablePath))
        {
            string jsonFile = File.ReadAllText(GamerBestExperiencePath());
            bestExperienceTable = JsonUtility.FromJson<GamerBestExperienceTable>(jsonFile);
        }
    }

    void SetGamerDataToHallOfHonerListsUI()
    {
        int i = 0;
        // Set Gamer enemies killed list to UI.
        foreach (EnemiesKilled killed in enemiesKilledTable.gamerkilledList)
        {
            i++;
            textUIGamersEnemiesKilledList.text += $"{i} Gamer: {killed.gamerName} killed: {killed.killed} {constCarrageReturn}";
        }

        i = 0;
        // Set Gamer best experience to UI.
        foreach (BestExperience gamerExperience in bestExperienceTable.gamerExperienceList)
        {
            i++;
            textUIGamersExperienceList.text += 
                $"{i} Gamer: {gamerExperience.gamerName} Level: {gamerExperience.level} Exp: {gamerExperience.experience} {constCarrageReturn}";
        }
    }

    string GamerEnemiesKilledTablePath() { return $"{Application.persistentDataPath}/GamerEnemiesKilled.json"; }
    string GamerBestExperiencePath() { return $"{Application.persistentDataPath}/GamerBestExperience.json"; }

}
