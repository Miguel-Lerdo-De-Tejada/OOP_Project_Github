using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class TeleportExp : MonoBehaviour
{
    [Space(3), Header("Teleport door information."), Space(3)]
    [SerializeField, Tooltip("Drag the teleport destination gameobject here to teletransport the player to its position.")]
    private Transform teleportDestination;
    [SerializeField, Tooltip("Write the area radius where the teleport door detects the player to teletransport him")]
    private float teleportDetectArea = 2.0f;
    [SerializeField, Tooltip("Write the time to wait until the player reaper in teleport destination")]
    private float reapearTime = 2.0f;
    [SerializeField, Tooltip("Teleport reapear effect particles")]
    GameObject teleportParticles;

    [Space(3), Header("Transportation between scenes.")]
    [SerializeField, Tooltip("Set to true whether the teleport door teletransport the player to a new scene.")]
    private bool isNewScene = false;
    [SerializeField, Tooltip("Next scene name, in case the teleport door teletransport the player to a new one.")]
    private string nextScene;
    [SerializeField, Tooltip("UI to load next scene")] 
    GameObject uiLoadNextScene;

    [Space(3), Header("Transportation between Local Scenarios."), Space(3)]
    [SerializeField, Tooltip("Old Scenario")] GameObject oldScenario;
    [SerializeField,Tooltip("New Scenario")] GameObject newScenario;

    private List<Collider> player = new List<Collider>();

    private void Start()
    {
        InitializeTeleporting();
    }

    // Teleport the player to another position.
    private void Update()
    {
        Teletransport();
        ExtendUpdate();
    }

    private void Teletransport()
    {
        if (IsPlayerDetected())
        {
            if (isNewScene)
            {
                UploadPlayerStatus();
                LoadSceneAsynchronously();
                // LoadScene();

                void UploadPlayerStatus()
                {
                    Status playerStatus = Sensor.GetNearbyGameObjects(Layers.player, transform.position, teleportDetectArea)[0].GetComponent<Status>();

                    SceneData.instance.UploadPlayerData(playerStatus);
                }
            }
            else
            {
                BeforeTeleporting();
                LocalPoint();                
            }
        }

        bool IsPlayerDetected() { return Sensor.Detect(Layers.player, transform.position, teleportDetectArea); }
    }

    void LoadScene() 
    {
        SceneManager.LoadScene(nextScene); 
    }

    void LoadSceneAsynchronously()
    {
        AsyncSceneLoader asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
        if (asyncSceneLoader)
        {
            if (uiLoadNextScene)
            {
                asyncSceneLoader.Load(uiLoadNextScene, nextScene);
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
        }
        else
        {
            if (uiLoadNextScene)
            {
                uiLoadNextScene.SetActive(true);
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    void LocalPoint()
    {
        List<Collider> players = new List<Collider>();
        players.AddRange(Sensor.GetNearbyColliders(Layers.player, transform.position, teleportDetectArea));

        foreach (Collider player in players)
        {
            Instantiate(teleportParticles, transform.position, Quaternion.identity);
            player.gameObject.SetActive(false);            
            StartCoroutine(DelayReaper(player.gameObject));
        }
    }

    IEnumerator DelayReaper(GameObject player)
    {
        yield return new WaitForSeconds(reapearTime);
        ChangeScenarios();
        player.transform.position = teleportDestination.position;
        player.SetActive(true);
        StopPlayer(player);

        if (teleportParticles) { EmmitParticles(); }

        AfterTeleporting();

        yield return null;

        void ChangeScenarios()
        {
            if (oldScenario) { oldScenario.SetActive(false); }
            if (newScenario) { newScenario.SetActive(true); }
        }
    }
    void EmmitParticles() 
    { 
        Instantiate(teleportParticles, teleportDestination.position, Quaternion.identity); 
    }

    void StopPlayer(GameObject player)
    {
        ThirdPersonShooterControllerExp playerController = player.GetComponent<ThirdPersonShooterControllerExp>();
        if (playerController) { playerController.StopMoving(); }
    }
    
    protected virtual void ExtendUpdate()
    {
        // It is usde for a Teleport door new functionality.
    }

    protected virtual void BeforeTeleporting()
    {
        // It is usde for a Teleport door to do things before teletransporting the player, like desactivate an enemy.
    }

    protected virtual void AfterTeleporting()
    {
        // It is used for a Teleport door to do things after teletransporting the player, like appear new enemies.
    }

    protected virtual void InitializeTeleporting()
    {
        // It is used to initialize Teleport door.
    }
}