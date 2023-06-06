using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSoulDialogue : Dialogue
{
    // This class inherits from Dialogue:
    // // Use the virtual methdod ShowDialgue() to manipulate the dialogue, the use of this method is obligatory;
    // Use input.talk attribute, to interactively begin a dialogue when player is near;
    // Use ShowDialoguePanel() and ShowMessages() methods inside the input talk condition to begin the dialogue between the Villager and the player.
    // Use the NextMessage() method to Change to the next villager message.
    // If you want the villager do something more, like offer something to the player, or harm the player you can check if the currentMessage is equeal or
    //      greater than the maxMassages attribute, if so, the villager could offer a gift to the player.

    protected override void ShowDialogue()
    {
        BeginDialogue();
    }

    void BeginDialogue()
    {
        bool isBeginingDialogue = CurrentMessage == 0;
        bool isDialoguePart = CurrentMessage <= MaxMessages;
        bool isInsideTheDialogue = CurrentMessage < MaxMessages;

        if (isBeginingDialogue)
        {
            BeguinCurrentDialogue();
        }
        else if (isDialoguePart)
        {
            if (input.talk)
            {
                if (isInsideTheDialogue)
                {
                    BeguinCurrentDialogue();
                }
                else
                {
                    NextMessage();
                }

                input.talk = false;
            }
        }
        else
        {            
            KillPlayer();
        }
    }

    protected override void HideMessages()
    {        
        base.HideMessages();
        KillPlayer();
    }

    void BeguinCurrentDialogue()
    {
        ShowUIDialoguePanel();
        ShowMessages();
    }

    void KillPlayer()
    {
        if (Player != null) { Player.GetComponent<Status>().death = true; }
    }
}
