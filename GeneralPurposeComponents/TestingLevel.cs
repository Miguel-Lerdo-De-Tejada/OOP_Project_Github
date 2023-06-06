using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLevel : MonoBehaviour
{
    [SerializeField, Tooltip("Level window")] LevelWindow levelWindow;
    [SerializeField,Tooltip("Level system")] LevelSystem levelSystem;

    private void Start()
    {

    }

    private void Update()
    {
        levelWindow.SetLevelSystem(levelSystem);
    }
}
