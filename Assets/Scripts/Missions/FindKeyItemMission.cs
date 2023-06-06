using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindKeyItemMission : ConditionalMission
{
    // This is a Find Object conditional mission, inherited from ConditionalMission.
    // Use the Inherited MissionStarts() method to add code when the mission begins.
    // Change to true the bool missionCondition if the findedGameObject is touched for the player.
    // Use the ConditionFinded() Method to mark the mission completed.

    [SerializeField, Tooltip("Key item name condition")] KeyItemName keyName;

    bool isMissionStart;
    Rewards reward;
    protected override void MissionStart()
    {
        base.MissionStart();
        isMissionStart = true;
        reward = GetComponent<Rewards>();
    }
    protected override void InProgress()
    {
        if (isMissionStart)
        {
            InicializeMission();
            isMissionStart = false;
        }

        if (AreThereKeyObjects())
        {
            if (IsSearchedObject())
            {
                ConditionFinded();                
            }
        }

        bool AreThereKeyObjects() { return KeyItems.keyItemsList.Count > 0; }
        base.InProgress();        
    }

    void InicializeMission()
    {
        if (!KeyItems.IsLoaded)
        {
            ClearKeyItems();
            ReadKeyItems();
        }
    }

    void ReadKeyItems() { KeyItems.AddKeyItems(); }

    void ClearKeyItems() { KeyItems.ClearKeyItems(); }

    bool IsSearchedObject() { return KeyItems.keyItemsList[(int)keyName]; }

    public int GetKeyItemIndex() { return (int)keyName; }

    public int ReadRewardToAdd() { return experienceReward; }

    public void ApplyReward(bool isKeyFinded) { reward.CheckMissionAccomplished(isKeyFinded, ReadRewardToAdd()); }

    public bool ReadRewardApplyed() { return reward.ReadRewardApplyed(); }

    public void SetRewardAppyed(bool isApplyed) { reward.SetRewardApplyed(isApplyed); }
}
