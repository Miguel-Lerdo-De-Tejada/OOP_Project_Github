using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalMission : Mission
{
    // This is a conditional Subbase mission, inherited from missions.
    // Use the method VerifyCondition, to verify if the condition is true.
    // If correntCondition is true, the mission is finish and you neeed to do so by invoking the FinishMission() method.

    [HideInInspector] public bool missionCondition = false;

    protected override void InProgress()
    {
        if (IsMissionCompleted()) { FinishMission(); }
    }

    bool IsMissionCompleted() { return missionCondition; }

    protected void ConditionFinded() { missionCondition = true; }
}
