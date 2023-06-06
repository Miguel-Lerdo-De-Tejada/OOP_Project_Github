using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftDialogueNew : DialogueManagerNew
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

    [SerializeField, Tooltip("Gift to offer.")] GameObject gift;

    protected override void ShowDialogue() 
    { 
        if (input) 
        {            
            if (IsTalkButtonPressed()) 
            {
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
        ShowGift(true);        
    }

    void ShowGift(bool isActive) { gift.SetActive(isActive); }

    public void SetShowGift(bool isGiftShowed) { ShowGift(isGiftShowed); SetFinishingDialogue(isGiftShowed); }

    public bool ReadShowGift() { return gift.activeSelf; }

    public bool IsFinishingDialogue() { return IsDialogueFinished; }
}
