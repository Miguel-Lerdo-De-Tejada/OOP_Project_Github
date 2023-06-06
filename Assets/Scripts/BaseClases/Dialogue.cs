using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public abstract class Dialogue : MonoBehaviour
{
    [Header("Dialogue")]
    // [SerializeField, Tooltip("NPC name")] string npcName;
    [TextArea(3,10)]
    [SerializeField, Tooltip("List of messages")] List<string> messages = new List<string>();
    [SerializeField, Tooltip("UI chat canvas game object")] GameObject UIDialogueCambas;
    [SerializeField, Tooltip("UI Text mes pro to show NPC name")] TextMeshProUGUI nameText;
    [SerializeField, Tooltip("UI Text mesh pro to show messages")] TextMeshProUGUI dialogueText;
    [SerializeField, Tooltip("Detect player radius")] float detectDistance = 2f;

    Status npcStatus;
    protected StarterAssetsInputs input;

    struct Tags { public static string player = "Player"; }

    struct DialogueAnimations
    {
        public static string Show = "ShowDialogue";
    }
    
    protected GameObject Player { get { return _player; } private set { value = _player; } }
    GameObject _player;

    protected int CurrentMessage { get { return _messageNumber; } private set { value = _messageNumber; } }
    protected int _messageNumber = 0;

    protected int MaxMessages { get { return _maxMessages; } private set { value = _maxMessages; } }
    int _maxMessages;

    private void Start()
    {
        OnDialogueStart();
        GetComponents();        
        GetMaxMassagesNumber();        
    }

    protected virtual void OnDialogueStart()
    {
        // Initialize other components, game objects or extras here.
    }

    private void LateUpdate()
    {
        BeginDialogue();
    }

    private void GetMaxMassagesNumber()
    {
        _maxMessages = messages.Count;
    }

    private void BeginDialogue()
    {        
        if (DetectPlayer())
        {
            ObtainPlayerGameObject();
            PlugPlayerInput();
            ShowDialogue();
        }
        else
        {            
            HideMessages();
            UnplugInput();
            DropPlayerGameObject();
        }
    }

    private void UnplugInput()
    {
        input = null;
    }

    private bool DetectPlayer()
    {
        bool playerDetected = Physics.CheckSphere(transform.position, detectDistance, Layers.player);
        return playerDetected;
    }

    private void ObtainPlayerGameObject()
    {
        _player = Physics.OverlapSphere(transform.position, detectDistance, Layers.player)[0].gameObject;
    }

    private void DropPlayerGameObject()
    {
        _player = null;
    }

    private void PlugPlayerInput()
    {
        input = Physics.OverlapSphere(transform.position, detectDistance, Layers.player)[0].GetComponent<StarterAssetsInputs>();
    }    

    protected void ShowUIDialoguePanel()
    {
        if (!UIDialogueCambas.activeSelf) { UIDialogueCambas.SetActive(true); }
    }

    protected void ShowMessages()
    {
        if (IsMessageInRange())
        {
            ShowName();
            ShowCurrentMassageByCharacters();
            NextMessage();
        }
        else
        {
            HideMessages();
        }        
    }

    bool IsMessageInRange() { return _messageNumber < _maxMessages; }

    protected void ShowName()
    {
        if (nameText != null)
        {
            if (nameText.text != npcStatus.actorName) { nameText.text = npcStatus.actorName; }
        }
    }

    protected void SetCurrentMessage(int message) 
    {
        if (message > 0 && message < _maxMessages) { _messageNumber = message; }
    }

    protected void ShowCurrentMassageByCharacters()
    {
        string currentMessage;
        StopAllCoroutines();
        if (nameText == null)
        {
            currentMessage = $"{npcStatus.actorName}: {messages[_messageNumber]}";
        }
        else
        {
            currentMessage = messages[_messageNumber];
        }

        StartCoroutine(WriteMessageByCharacters(currentMessage));        
    }

    protected void NextMessage()
    {
        _messageNumber++;
    }

    protected virtual void HideMessages()
    {
        if (input != null) { input.talk = false; }
        _messageNumber = 0;
        nameText.text = "";
        dialogueText.text = "";
        UIDialogueCambas.SetActive(false);        
    }

    IEnumerator WriteMessageByCharacters(string message)
    {
        dialogueText.text = "";        
        foreach (char letter in message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void GetComponents()
    {
        npcStatus = GetComponent<Status>();        
    }

    // Abstract methods, the use in child duialogue classes is obligatory.
    protected abstract void ShowDialogue();
}
