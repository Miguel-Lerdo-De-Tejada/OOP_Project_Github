using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(DialogueComponent))]
[RequireComponent(typeof(Status))]
public abstract class DialogueManagerNew : MonoBehaviour
{
    // DialogueManager attributes:

    // NPC Canvas
    #region
    [Space(3),Header("NPC data:"),Space(3)]
    [SerializeField, Tooltip("Dialogue canvas for npc")]
    GameObject dialogueCanvasNPC;
    [SerializeField, Tooltip("UI npc Photo")]
    GameObject npcPhotoArea;
    [SerializeField, Tooltip("NPC name text area")]
    TextMeshProUGUI npcNameTextArea;
    [SerializeField, Tooltip("UI npc text area")]
    TextMeshProUGUI npcTextArea;
    #endregion

    // Player Canvas
    #region
    [Space(6),Header("Player dialogue canvas:"),Space(3)]
    [SerializeField, Tooltip("Dialogue canvas for player")]
    GameObject dialogueCanvasPlayer;
    [SerializeField, Tooltip("UI npc Photo")]
    GameObject playerPhotoArea;
    [SerializeField, Tooltip("Character name in the dialogue")]
    TextMeshProUGUI playerNameTextArea;
    [SerializeField, Tooltip("UI npc text area")]
    TextMeshProUGUI playerTextArea;
    #endregion

    // NPC detection area
    #region
    float detectionRange;
    #endregion

    // Components
    #region
    DialogueComponent npcDialogue;
    GameObject playerGameObject;
    Status npcStatus;
    protected Status playerStatus;
    RawImage npcRawPhoto;
    RawImage playerRawPhoto;
    protected StarterAssetsInputs input;
    ThirdPersonController playerController;
    #endregion

    // Dialogue 
    #region
    [SerializeField, Tooltip("Seconds to wait next letter in letter by letter message."), Range(0.001f, 0.1f)] float messageLetterWaitTime = 0.001f;
    public bool IsDialogueFinished { get { return isDialogueFinished; } private set { } }
    public bool isTalk;
    bool isDialogueFinished = false;
    protected int currentDialogue;
    float playerSpeed;
    int talkPressed;
    
    // Constants
    const int cFreez = 0;
    const int cFirstDialogue = 0;
    struct talkState
    {
        public const int inactive = 0;
        public const int pressed = 1;
        public const int doublePressed = 2;
    }
    
    #endregion

    // Unity Events
    #region
    private void Awake()
    {
        GetComponents();
        OnDialogueAwake();
    }
    
    void LateUpdate()
    {
        ActivateDialogue();
    }
    #endregion

    // Awake:
    #region 
    void GetComponents()
    {
        GetNPCComponents();

        void GetNPCComponents()
        {            
            npcStatus = GetComponent<Status>();
            npcRawPhoto = npcPhotoArea.GetComponent<RawImage>();
            npcDialogue = GetComponent<DialogueComponent>();
            detectionRange = ReadPlayerDetectionRange();

            float ReadPlayerDetectionRange() 
            {
                if (GetComponent<VillagerPatrolExp>()) 
                { 
                    return GetComponent<VillagerPatrolExp>().playerDetectionRange; 
                } 
                else if (GetComponent<VillagerPatrol>())
                {
                    return GetComponent<VillagerPatrol>().PlayerDetectionDistance();
                }
                else
                {
                    return 1;
                }

            }
        }
    }
    #endregion

    // Update
    #region
    void ActivateDialogue()
    {
        if (IsTalkInactive())
        {
            if (DetectPlayer())
            {
                GetPlayerComponents();
                ShowDialogue();
            }
        }

        bool DetectPlayer() { return Sensor.Detect(Layers.player, transform.position, detectionRange); }        
    }
    #endregion

    // Protected Methods for inheritance:
    #region
    protected bool IsTalkButtonPressed() 
    {        
        bool pressed = input.talk;
        if (!IsPlayerBussy()) { UnpressTalkButton(); }

        return pressed; 
    }
    protected bool IsTalkInactive() { return talkPressed == talkState.inactive; }
    protected int CurrentMessage() { return currentDialogue; }
    protected void UnpressTalkButton() { input.talk = false; }
    protected void ShowDialogueMessages()
    {
        // Debug.Log($"NPC {npcStatus.actorName} intending to dialogue.");
        if (!IsPlayerBussy())
        {            
            if (IsDialogueActive())
            {
                SetPlayerBussy();
                SetTalkToPressedState();
                SetPublicDialogueActive();
                FreezPlayer();                
                ShowDialogueByTurns();                

                void SetTalkToPressedState() { talkPressed = talkState.pressed; }
                void SetPublicDialogueActive() { isTalk = true; }
                void FreezPlayer() { if (playerController) { playerController.MoveSpeed = cFreez; } }
                void SetPlayerBussy()
                {
                    playerController.isBussy = true;
                    playerController.npcTalking = npcStatus.actorName;
                }
                void ShowDialogueByTurns()
                {
                    // Read current dialogue.
                    if (npcDialogue)
                    {
                        
                        if (IsNPCDialogueTurn())
                        {
                            ShowNPCDialogueUI();

                            void ShowNPCDialogueUI()
                            {
                                dialogueCanvasPlayer.SetActive(false);
                                dialogueCanvasNPC.SetActive(true);

                                npcRawPhoto.texture = npcStatus.actorSpriteImage;
                                npcNameTextArea.text = npcStatus.actorName;                                
                                ShowDialogueByLetter(npcTextArea, npcDialogue.Read().dialogue[currentDialogue]);                                
                            }
                        }
                        else if (IsPlayerDialogueTurn())
                        {
                            ShowPlayerDialogueUI();

                            void ShowPlayerDialogueUI()
                            {
                                dialogueCanvasPlayer.SetActive(true);
                                dialogueCanvasNPC.SetActive(false);

                                playerRawPhoto.texture = playerStatus.actorSpriteImage;
                                playerNameTextArea.text = playerStatus.actorName;                                
                                ShowDialogueByLetter(playerTextArea, npcDialogue.Read().dialogue[currentDialogue]);                                
                            }
                        }

                        void ShowDialogueByLetter(TextMeshProUGUI dialogueArea, string currentMessage)
                        {
                            StartCoroutine(WriteMessageByCharacters(dialogueArea, currentMessage));
                        }
                        bool IsNPCDialogueTurn() { return npcDialogue.Read().turn[currentDialogue] == DialogueTurn.npc; }
                        bool IsPlayerDialogueTurn() { return npcDialogue.Read().turn[currentDialogue] == DialogueTurn.player; }
                    }
                }
            }
            else
            {
                FinishDialogue();
            }                        
        }        
    }

    bool IsPlayerBussy()
    {
        if (playerController.npcTalking != npcStatus.actorName)
        {
            return playerController.isBussy;
        }
        else
        {
            return false;
        }
    }
    protected void AddTalkButtonPressed() 
    {        
        talkPressed++;
        if (talkPressed > talkState.doublePressed) { talkPressed = talkState.inactive; }
    }
    protected void GetPlayerComponents()
    {
        GetPlayerGameObject();

        if (playerGameObject)
        {
            GetPlayerStats();
            GetPlayerInput();
            GetPlayerController(); // to stop player when talks.


            void GetPlayerStats()
            {
                if (playerStatus == null) { playerStatus = ReadPlayerStats(); }
                if (playerRawPhoto == null) { playerRawPhoto = playerPhotoArea.GetComponent<RawImage>(); }

                Status ReadPlayerStats()
                {
                    return playerGameObject.GetComponent<Status>();
                }
            }
            void GetPlayerInput()
            {
                input = playerGameObject.GetComponent<StarterAssetsInputs>();
            }
            void GetPlayerController()
            {
                playerController = playerGameObject.GetComponent<ThirdPersonController>();
                if (playerController.MoveSpeed > cFreez) playerSpeed = playerController.MoveSpeed;
            }
        }

        void GetPlayerGameObject() { playerGameObject = Sensor.GetNearbyGameObjects(Layers.player, transform.position, detectionRange)[0]; }
    }
    protected bool ReadFinishingDialogue() { return isDialogueFinished; }
    protected void SetFinishingDialogue(bool IsFinished) { isDialogueFinished = IsFinished; }

    #endregion

    // Virtual and Abstract methods for inherited components:
    #region    
    protected abstract void ShowDialogue();

    protected virtual void OnDialogueAwake()
    {
        // Initialize other components, game objects or extras here.
    }

    protected virtual void FinishDialogue()
    {
        FinishDialogue();
        UnfreezPlayer();
        UnsetPublicDailogue();
        UnsetPlayerBussy();
        DropPlayerController();

        void FinishDialogue()
        {
            dialogueCanvasNPC.SetActive(false);
            dialogueCanvasPlayer.SetActive(false);
            currentDialogue = cFirstDialogue;
            talkPressed = talkState.inactive;
            if (!isDialogueFinished) { isDialogueFinished = true; }
        }
        void UnfreezPlayer() { playerController.MoveSpeed = playerSpeed; }
        void UnsetPlayerBussy() 
        {
            playerController.isBussy = false;
            playerController.npcTalking = string.Empty;
        }
        void DropPlayerController() { playerController = null; }
        void UnsetPublicDailogue() { isTalk = false; }
    }    

    protected virtual bool IsDialogueActive() { return currentDialogue < npcDialogue.Read().dialogue.Count; }
    #endregion

    // Coroutines:
    #region
    IEnumerator WriteMessageByCharacters(TextMeshProUGUI dialogueArea, string message)
    {
        dialogueArea.text = string.Empty;
        
        foreach (char letter in message.ToCharArray())
        {            
            if (IsTalkButtonPressed()) { AddTalkButtonPressed(); }            
            if (talkPressed >= talkState.doublePressed)
            {                
                StopCoroutine(WriteMessageByCharacters(dialogueArea, message));
                break; 
            }

            dialogueArea.text += letter;
            yield return new WaitForSeconds(messageLetterWaitTime);
        }
        RestartTalkPressed();
        SetMessage();
        IncrementDialogueIndex();        
        yield return null;

        void RestartTalkPressed() { talkPressed = talkState.inactive; }
        void SetMessage()
        {
            dialogueArea.text = string.Empty;
            dialogueArea.text = message;
        }
        void IncrementDialogueIndex() { currentDialogue++; }
    }
    #endregion
}