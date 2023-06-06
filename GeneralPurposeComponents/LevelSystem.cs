using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : MonoBehaviour 
{
    Status status;

    // Unity events
    void Start() { GetComponents(); }

    // Public methods:
    public void AddExperience(int amount)
    {
        status.experience += amount;

        if (IsNextLevel()) { addLevel(); }
    }
    public int GetLevelNumber() { if (status) { return status.level; } else { return 0; } }
    public int GetExperience() { if (status) { return status.experience; } else { return 0; } }
    public int GetExperienceNextLevel() { return status.experienceNextLevel; }
    public int GetResurrections() { return status.resurrections; }

    // Private

    void GetComponents() { status = GetComponent<Status>(); }

    bool IsNextLevel() { return status.experience >= status.experienceNextLevel; }

    void addLevel()
    {
        int currentExperience = status.experience;
        while (!isAddingLevelFinished())
        {
            status.level++;
            status.experience -= status.experienceNextLevel;
            status.experienceNextLevel *= status.experienceGrouthRate;            
        }
        Debug.Log($"Experience {status.experience}, Next level {status.experienceNextLevel}.");
    }

    bool isAddingLevelFinished() { if (status.experience - status.experienceNextLevel <= 0) { return true; } else { return false; } }
}
