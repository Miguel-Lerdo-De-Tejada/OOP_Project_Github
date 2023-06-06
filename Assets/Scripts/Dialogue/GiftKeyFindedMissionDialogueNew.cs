using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftKeyFindedMissionDialogueNew : DialogueManagerNew
{
    // INHERITANCE
    /* 
    * This class inherits from DialogueManagerNew:
    * 
    * To initialize aditional objects or components in this object, you can use the OnDialogueAwake to do so.
    * Use the virtual methdod ShowDialgue() to manipulate the dialogue, the use of this method is obligatory;
    * Test if the input component is set in the player using the if input sentence.
    * If it is set, use IsTalkButtonPressed function, to interactively begin a dialogue when player is near and the talk button is pressed;    
    * ShowDialogueMessages() method inside the IsTalkButtonPressed function condition to beguin the dialogue between the Villager and the player.    
    * If you want the villager do something more, like offer something to the player, or harm the player you can use the Finish Dialogue method to do
    * so.
    * If you want to change the conditon to finish the dialogue, you can override IsDialogueActive function condition to do so.
    */

    [SerializeField, Tooltip("Find key item mission.")] FindKeyItemMission findMission;
    [SerializeField, Tooltip("Gift to show")] GameObject gift;
    [SerializeField, Tooltip("Dialogue Index when player finds the key."), Range(1, 100)] int keyFindedDialogueIndex;    

    protected override void ShowDialogue() 
    {
        if (IsKeyFinded()) 
        {            
            if (IsDialogueBellowKeyFindedDailogue()) { SetFirstDialogueToKeyFindedDialogueIndex(); }
            
        }
        if (input) 
        {
            if (IsTalkButtonPressed()) 
            {
                if (IsKeyFinded()) 
                { 
                    HideUIKeyItem();
                    ApplyReward();

                    void ApplyReward()
                    {
                        if(!findMission.ReadRewardApplyed()) findMission.ApplyReward(IsKeyFinded());
                    }
                }

                ShowDialogueMessages(); 
            } 
        } 
    }

    protected override void OnDialogueAwake()
    {
        base.OnDialogueAwake();
        ShowGift(false);        
    }

    protected override void FinishDialogue()
    {
        base.FinishDialogue();
        if (IsKeyFinded())
        {
            ShowGift(true);            
            SetFirstDialogueToKeyFindedDialogueIndex();            
        }
    }

    protected override bool IsDialogueActive()
    {
        if (IsKeyFinded())
        {
            return base.IsDialogueActive();
        }
        else
        {
            return IsDialogueBellowKeyFindedDailogue();
        }
    }

    void ShowGift(bool isActive) { if (gift) { gift.SetActive(isActive); } }

    void SetFirstDialogueToKeyFindedDialogueIndex() { currentDialogue = keyFindedDialogueIndex; }

    protected bool IsKeyFinded() { return findMission.isFinished; }

    bool IsDialogueBellowKeyFindedDailogue() { return currentDialogue < keyFindedDialogueIndex; }

    void HideUIKeyItem()
    {
        UIKeyItemsManager uiKeyItemManager = GameObject.FindObjectOfType<UIKeyItemsManager>();

        uiKeyItemManager.SetActive(findMission.GetKeyItemIndex(), false);
    }

    public bool ReadGiftState() { if (gift) { return gift.activeSelf; } else { return false; } }

    public void SetGiftState(bool isActive)
    {
        if (gift) 
        {            
            gift.SetActive(isActive);
            if (isActive) { HideUIKeyItem(); }
        } 
    }

    public int KeyFindedIndex() { return findMission.GetKeyItemIndex(); }

    public bool ReadKeyFindedRewardApplyed() { return findMission.ReadRewardApplyed(); }

    public void SetKeyFindedRewardApplyed(bool isApplyed) { findMission.SetRewardAppyed(isApplyed); }
}
