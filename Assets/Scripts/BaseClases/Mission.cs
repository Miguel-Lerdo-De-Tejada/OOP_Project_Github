using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    // Mission allows to create many types of game goals depending the task you want to launch.
    // For example, missions of counting , Get an object or reach some place.
    // Use the InProgress obligatory abstract method to defien the rules of your mission, and use the flag isFinished when the mission is achieved.

    // Rewards:
    [SerializeField, Tooltip("Experience reward")] protected int experienceReward;

    [HideInInspector] public bool isFinished = false;

    private void Start()
    {
        MissionStart();
    }

    // Update is called once per frame
    void Update()
    {
        InProgress();
    }

    protected void FinishMission()
    {
        isFinished = true;
    }

    protected virtual void MissionStart()
    {
        // Do things like get components or deactive gifts when mission starts.
    }

    // Define the ruls of the mission in this abstract class.
    protected abstract void InProgress();
}
