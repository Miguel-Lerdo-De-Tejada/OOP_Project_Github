using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingMission : Mission
{
    // This is a counting Subbase mission, inherited from missions.
    // Use the method Add, to add one to the currentCount variable.
    // If correnCount is equeal or biger than MaxCount, the mission is finish and you neeed to do so by set the isFinished flag to true.
    [Header("Count mission")]
    [SerializeField, Tooltip("Maximun count to achieve the gol")] int maxCount;
    [HideInInspector] int currentCounting = 0;

    protected override void InProgress()
    {
        CheckIsFinished();
    }

    public void CheckIsFinished()
    {        

        if (IsMissionCompleted()) 
        {
            FinishMission();
            RestartCount();
        }
    }

    bool IsMissionCompleted() { return currentCounting >= maxCount; }    

    public void AddCount() 
    { 
        currentCounting++;        
    }

    public void RestartCount()
    {
        currentCounting = 0;
    }

    public void SetCount(int loadedCount)
    {
        loadedCount = loadedCount < 0 ? 0 : loadedCount;
        currentCounting = loadedCount;
    }

    protected int GetCurrentCount() { return currentCounting; }

    protected int GetMaxCount() { return maxCount; }
}
