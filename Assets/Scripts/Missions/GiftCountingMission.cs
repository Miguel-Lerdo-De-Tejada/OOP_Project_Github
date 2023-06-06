using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCountingMission : CountingMission
{
    // GiftCountingMission Hinerits form ContingMission.
    // Create an empty game object in Unity which name is GiftCountingMission and attach this component to it.
    // Use the InProgress virtual method to offer the gift.
    // Do not delete base.InProgress() from the InProgress virtual methdo, if so, the method do not count.
    // Use the isFinished flag to check the mission is acomplished, if it is, show the gift.

    [SerializeField, Tooltip("Gift to offer")] GameObject gift;
    Rewards reward;

    private void Awake()
    {
        reward = GetComponent<Rewards>();
        RetrieveCountingMissionRewardApplayed();

        void RetrieveCountingMissionRewardApplayed()
        {
            reward.SetRewardApplyed(SceneData.instance.UpdateCountingMissionRewardAdded());
        }
    }

    protected override void MissionStart()
    {
        HideGift();        
    }
    protected override void InProgress()
    {
        base.InProgress();        
        if (isFinished) 
        { 
            ShowGift();
            ApplyReward();

            void ApplyReward()
            {
                // reward = GetComponent<Rewards>();
                if (!reward.ReadRewardApplyed()) { reward.CheckMissionAccomplished(isFinished, experienceReward); }
            }
        }
    }

    void ShowGift()
    {
        gift.SetActive(true);
    }

    void HideGift()
    {
        gift.SetActive(false);
    }

    public int GetCount()
    {
        if (gift.activeSelf) { return GetMaxCount(); } else { return GetCurrentCount(); }
    }

    public bool ReadRewardApplyed() 
    {
        // reward = GetComponent<Rewards>();
        return reward.ReadRewardApplyed(); 
    }

    public void SetRewardAppyed(bool isApplyed) 
    {
        // reward = GetComponent<Rewards>();
        reward.SetRewardApplyed(isApplyed); 
    }

}
