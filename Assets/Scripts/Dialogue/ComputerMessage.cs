using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
using StarterAssets;

public class ComputerMessage : MonoBehaviour
{
    [Header("Scriptable message object")]
    [SerializeField, Tooltip("Set the computer message scriptable object here")] MessageComponent messageComponent;
    [Header("Key to throw")]
    [SerializeField, Tooltip("Drag a key if the computer offers one.")] private GameObject doorKey;
    [SerializeField, Tooltip("Set the position to instantiate key")] private Transform spawnPosition;
    [SerializeField, Tooltip("Set the number of keys which apear in the desk in turns.")] private int turns;
    [Header("User Interfaces to incteract")]
    [SerializeField, Tooltip("Set the UIComputer here")] GameObject computerUI;
    [SerializeField, Tooltip("Device descriptor when the device detects the player.")] GameObject deviceDescriptor;
    [SerializeField, Tooltip("Set the text mesh pro computer messages here.")] TextMeshProUGUI computerMessages;
    [Header("Device attributes:")]
    [SerializeField, Tooltip("Set the computer name.")] string computerName;
    [SerializeField, Tooltip("Detect player range")] float detectPlayer = 2.0f;

    StarterAssetsInputs input;
    
    int currentMessage = 0;
    int talkPressed = 0;
    bool isKeyOffered = false;
    string currentDeviceName = string.Empty;

    struct talkState
    {
        public const int inactive = 0;
        public const int pressed = 1;
        public const int doublePressed = 2;
    }



    // Unity events:    

    private void LateUpdate()
    {
        if (DetectPlayer())
        {
            ShowDeviceDescriptor();
            PlugPlayerInput();
            AccessComputer();

            void ShowDeviceDescriptor() 
            {
                if (deviceDescriptor)
                { 
                    currentDeviceName = messageComponent.Read().computerName;
                    deviceDescriptor.GetComponentInChildren<TextMeshProUGUI>().text =
                        $"This is {currentDeviceName}, I could obtain valuable information stored on it.";
                    deviceDescriptor.SetActive(true);
                }
            }
        }
        else
        {
            HideDeviceDescriptor();
            UnplugInput();

            void HideDeviceDescriptor()
            {
                if (deviceDescriptor && messageComponent.Read().computerName == currentDeviceName)
                {
                    deviceDescriptor.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                    currentDeviceName = string.Empty;
                    deviceDescriptor.SetActive(false);
                }
            }
        }
    }

    // Public methods

    public void ShowKey()
    {
        bool isLoaded = false;
        if (doorKey != null)
        {
            isKeyOffered = true;
            ThrowKey(isLoaded);
        }
    }

    public bool ReadKeyState()
    {        
        return isKeyOffered;
    }

    public int ReadTurns()
    {
        return turns;
    }

    public void SetKeyState(bool isLoaded, int nextTurn)
    {
        isKeyOffered = isLoaded;
        if (isLoaded) 
        {
            turns = nextTurn;
            ThrowKey(isLoaded);
        }
    }

    public void TurnOffComputer()
    {
        if (computerUI.activeSelf) computerUI.SetActive(false);
        currentMessage = 0;
        talkPressed = 0;
        input.talk = false;
    }


    // Protected methods
    protected virtual void AccessComputer()
    {
        if (IsTalkPressed())
        {
            if (talkPressed == talkState.inactive)
            {
                talkPressed = talkState.pressed;
                ShowCurrentMessage();
                ShowKey();
            }
        }
    }

    protected void ShowCurrentMessage()
    {
        if (computerUI != null)
        {
            TurnOnComputer();            
            if (currentMessage >= messageComponent.Read().computerMessages.Count)
            {
                TurnOffComputer();
            }
            else
            {
                ShowCurrentMessageByCharacters();                
            }
        }
    }

    protected void ShowCurrentMessageByCharacters()
    {
        StopAllCoroutines();
        string messageSended = $"{messageComponent.Read().computerName}: {messageComponent.Read().computerMessages[currentMessage]}";

        StartCoroutine(WriteMessageByCharacters(messageSended));
    }

    // Privated methods:
    void UnplugInput()
    {
        if (input)
        {
            if (input.talk) { input.talk = false; }
            input = null;
        }
    }

    bool DetectPlayer()
    {
        return Physics.CheckSphere(transform.position, detectPlayer, Layers.player);
    }

    void PlugPlayerInput()
    {
        if (input == null) { input = Physics.OverlapSphere(transform.position, detectPlayer, Layers.player)[0].GetComponent<StarterAssetsInputs>(); }
    }

    void ThrowKey(bool isLoaded)
    {        
        if (turns > 0) 
        {
            if (spawnPosition)
            {
                Instantiate(doorKey, spawnPosition.position, Quaternion.identity);
                turns--;
            }
        }
        else if (isLoaded)
        {
            if (spawnPosition) { Instantiate(doorKey, spawnPosition.position, Quaternion.identity); }
        }
    }

    void DropLastMessage()
    {
        computerMessages.text = string.Empty;
    }

    void TurnOnComputer()
    {
        if (!computerUI.activeSelf) { computerUI.SetActive(true); }
    }

    bool IsTalkPressed()
    {
        bool pressed = input.talk;
        input.talk = false;
        return pressed;
    }

    // Corroutines:

    IEnumerator WriteMessageByCharacters(string message)
    {
        DropLastMessage();
        foreach (char letter in message.ToCharArray())
        {
            if (IsTalkPressed()) { AddTalkPressed(); }
            if (talkPressed >= talkState.doublePressed) { break; }

            computerMessages.text += letter;
            yield return null;
        }
        
        RestartTalkPressed();
        SetMessage();
        NextMessage();

        void AddTalkPressed() { talkPressed++; }
        void RestartTalkPressed() { talkPressed = talkState.inactive; }
        void SetMessage() { computerMessages.text = message; }
        void NextMessage() { currentMessage++; }
    }
}


