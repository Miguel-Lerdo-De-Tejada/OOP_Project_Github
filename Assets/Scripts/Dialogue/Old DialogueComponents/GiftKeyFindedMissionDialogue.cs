using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftKeyFindedMissionDialogue : Dialogue 
{
    // This class inherits from Dialogue:
    // Use the virtual methdod ShowDialgue() to manipulate the dialogue, the use of this method is obligatory;
    // Use the virtual method onDialogueStart() to initialize something, like hide a gift in the Scene when the game starts.
    // Use input.talk attribute, to interactively begin a dialogue when player is near;
    // Use ShowDialoguePanel() and ShowMessages() methods inside the input talk condition to begin the dialogue between the Villager and the player.
    // Use the NextMessage() method to Change to the next villager message.
    // If you want the villager do something more, like offer something to the player, or harm the player you can check if the CurrentMessage is equeal or
    //      greater than the MaxMassages attribute, if so, the villager could offer a gift to the player.

    [SerializeField, Tooltip("Gift counting mission.")] FindKeyItemMission findMission;
    [SerializeField, Tooltip("Gift to show")] GameObject gift;
    [SerializeField, Tooltip("Index of beginning message when Key is finded"), Range(1, 10)] int keyFindedMessages = 1;
    
    int currentKFMessage;
    int maxKFMessages;

    bool isKeyFinded;
    bool isDialogueStarting;

    protected override void OnDialogueStart()
    {
        Debug.Log("Gift key finded dialogue.");
        isDialogueStarting = true;
        base.OnDialogueStart();
    }

    protected override void ShowDialogue()
    {
        if (input.talk)
        {
            ObtainDialoguePositions();            
            BeguinCurrentDialogue();            
            if (IsDialogueFinished()) {if (IsKeyItemFinded()) { ShowGift(); }}
            input.talk = false;
        }

        if (isDialogueStarting)
        {
            HideGift();
            isDialogueStarting = false;
        }
    }

    public bool ReadGiftState() { if (gift) { return gift.activeSelf; } else { return false; } }

    public void SetGiftState(bool isLoaded, bool isShowGift)
    {
        isKeyFinded = isLoaded;

        if (isKeyFinded)
        {
            FirstMessage();
            ObtainDialoguePositions();
        }
        
        if (isShowGift) 
        {            
            ShowGift();            
        }
        else
        {
            HideGift();
        }        
    }

    // Return if the intermediate key item is finded to show the dialogue afte you find the key item in a Gift key finded mission
    public int KeyFindedIndex()
    {
        int keyItem = findMission.GetKeyItemIndex();

        return keyItem;
    }

    bool IsDialogueFinished() { return CurrentMessage >= MaxMessages; }

    bool IsKeyItemFinded() 
    {        
        if (!isKeyFinded) { isKeyFinded = findMission.isFinished; }     
        return isKeyFinded; 
    }

    void BeguinCurrentDialogue()
    {
        ShowUIDialoguePanel();
        ShowKeyFindedMessages();
    }

    void ShowKeyFindedMessages()
    {        
        if (IsMessageInRange())
        {            
            ShowName();
            ShowCurrentMassageByCharacters();
            NextKFMessage();            
        }
        else
        {            
            HideMessages();
            SetFirstCurrentMessage();
        }
    }

    bool IsMessageInRange() { return currentKFMessage < maxKFMessages; }

    void ShowGift() { if (gift) { gift.SetActive(true); } }

    void HideGift() { if (!isKeyFinded) { if (gift) { gift.SetActive(false); } } }

    void ObtainDialoguePositions()
    {        
        if (IsKeyFindedMessage())
        {
            bool isFinded = IsKeyItemFinded();            
            if (isFinded)
            {
                ApplayKeyFindedMessagesBundaries();
                LoadMaxMessagesValue();
                FirstMessage();
            }
            else
            {
                SetMaxMessagesBeforeKeyFinded();
            }            
        }        
    }

    bool IsKeyFindedMessage() { return keyFindedMessages > 0; }

    void LoadMaxMessagesValue() { maxKFMessages = MaxMessages; }

    void ApplayKeyFindedMessagesBundaries() { currentKFMessage = (int)Mathf.Clamp(CurrentMessage, keyFindedMessages, maxKFMessages); }

    void SetMaxMessagesBeforeKeyFinded() { maxKFMessages = keyFindedMessages; }

    void NextKFMessage() 
    {
        NextMessage();
        SetCurrentKeyFindedMessage();        
    }

    void SetFirstCurrentMessage()
    {
        if (IsKeyItemFinded())
        {
            SetCurrentMessage(keyFindedMessages);            
        }

        SetCurrentKeyFindedMessage();
    }

    void FirstMessage()
    {
        if (IsKeyItemFinded())
        {
            if (IsFirstKFCurrentMessage()) { SetFirstCurrentMessage(); }
        }
    }

    bool IsFirstKFCurrentMessage() { return CurrentMessage < keyFindedMessages; }

    void SetCurrentKeyFindedMessage() { currentKFMessage = CurrentMessage; }
}
