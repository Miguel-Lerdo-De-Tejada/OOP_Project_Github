using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftDialogue : Dialogue
{
    // This class inherits from Dialogue:
    // Use the virtual methdod ShowDialgue() to manipulate the dialogue, the use of this method is obligatory;
    // Use the virtual method onDialogueStart() to initialize something, like hide a gift in the Scene when the game starts.
    // Use input.talk attribute, to interactively begin a dialogue when player is near;
    // Use ShowDialoguePanel() and ShowMessages() methods inside the input talk condition to begin the dialogue between the Villager and the player.
    // Use the NextMessage() method to Change to the next villager message.
    // If you want the villager do something more, like offer something to the player, or harm the player you can check if the CurrentMessage is equeal or
    //      greater than the MaxMassages attribute, if so, the villager could offer a gift to the player.

    [SerializeField,Tooltip("Gift to offer.")] GameObject gift;
    
    bool isFinishingDialogue;

    protected override void ShowDialogue()
    {
        if (!isFinishingDialogue) { isFinishingDialogue = CurrentMessage >= MaxMessages; }

        // if (_isFinishingDialogue) { Debug.Log($"Is finising dialogue outside: {IsFinishingDialogue}."); }
        if (input.talk)
        {
            BeguinCurrentDialogue();
            if (isFinishingDialogue){ ShowGift(); }
            input.talk = false;
        }
    }

    protected override void OnDialogueStart()
    {
        if (!isFinishingDialogue) { HideGift(); }
    }

    void BeguinCurrentDialogue()
    {
        ShowUIDialoguePanel();
        ShowMessages();
    }

    void ShowGift()
    {        
        gift.SetActive(true);        
    }

    void HideGift()
    {
        gift.SetActive(false);
    }

    public void SetShowGift(bool showGift)
    {        
        gift.SetActive(showGift);
        isFinishingDialogue = showGift;
    }

    public bool IsFinishingDialogue() { return isFinishingDialogue; }
}

