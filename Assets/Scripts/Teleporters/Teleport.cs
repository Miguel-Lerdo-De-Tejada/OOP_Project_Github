using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Header("Teleporter configuration")]
    [SerializeField, Tooltip("Drag the teleport gameobject here to teletransport the player to its position.")] private Transform teleportPosition;
    [SerializeField, Tooltip("Drag the scene you leave here if you intend to leave one.")] private GameObject oldScene;
    [SerializeField, Tooltip("Drag the scene you enter here if you intend to enter new one.")] private GameObject newScene;
    [SerializeField, Tooltip("Set if the teleport is the game win objective.")] private bool isWinGoal = false;
    [SerializeField, Tooltip("Write win scene name.")] private string winScene;
    [Header("UI")]
    [SerializeField, Tooltip("Drag NPCs Descriptor UI game object here")] GameObject npcsDescriptor;
    [SerializeField, Tooltip("Load Scene UI")] GameObject uiLoadNextScene;

    private const string cPlayerTag = "Player";

    // Teleport the player to another position.
    private void OnTriggerStay(Collider other)
    {
        DeactivateDescriptor();
        ChangeScene(other);

        void DeactivateDescriptor() 
        { 
            if (npcsDescriptor) 
            { 
                if (npcsDescriptor.activeSelf) { npcsDescriptor.SetActive(false); }
            }
            else
            {
                Debug.LogError("Attach a npc Descriptor UI on this component for its appropriate functionality.");
                Debug.Break();
            }
        }
    }

    private void ChangeScene(Collider player)
    {
        if (player.gameObject.CompareTag(cPlayerTag))
        {
            if (!isWinGoal)
            {                
                TeleportPlayer(player);
            }
            else
            {
                UploadPlayerStatus();
                UploadPlayerResurrections();                
                LoadSceneAsynchronously();

                void UploadPlayerStatus()
                {
                    Status playerStatus = player.GetComponent<Status>();

                    SceneData.instance.UploadPlayerData(playerStatus);
                }
                void UploadPlayerResurrections() 
                { 
                    SceneData.instance.resurrections = player.gameObject.GetComponent<Status>().resurrections;
                    SceneData.instance.isResurrection = true;
                }
                void LoadSceneAsynchronously()
                {
                    AsyncSceneLoader asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
                    if (asyncSceneLoader)
                    {
                        if (uiLoadNextScene)
                        {
                            asyncSceneLoader.Load(uiLoadNextScene, winScene);
                        }
                        else
                        {
                            SceneManager.LoadScene(winScene);
                        }
                    }
                    else
                    {
                        if (uiLoadNextScene)
                        {
                            uiLoadNextScene.SetActive(true);
                            SceneManager.LoadScene(winScene);
                        }
                        else
                        {
                            SceneManager.LoadScene(winScene);
                        }
                    }
                }
            }
        }
    }

    private void TeleportPlayer(Collider player)
    {
        player.gameObject.SetActive(false);
        IsEnterNewScene();
        player.transform.position = teleportPosition.position;
        player.gameObject.SetActive(true);

    }

    private void IsEnterNewScene()
    {
        if ( oldScene != null)
        {
            if(newScene != null)
            {
                oldScene.SetActive(false);
                newScene.SetActive(true);
            }
        }
    }
}
