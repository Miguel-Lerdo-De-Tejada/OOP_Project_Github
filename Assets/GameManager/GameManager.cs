using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
// using UnityEditorInternal.Profiling.Memory.Experimental;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("User Interface Menu")] GameObject uiMenu;
    [SerializeField, Tooltip("Player status ui Window")] GameObject uiStatus;
    [SerializeField, Tooltip("Database game object")] GameObject dataBaseObject;
    [SerializeField, Tooltip("Load and New UI to wait scenes")] List<GameObject> uiInterfacesToWait = new List<GameObject>();
    [SerializeField, Tooltip("Is the scene a training room")] bool isTrainingRoom;

    GameObject player;
    bool isPlayerRunned;

    List<Collider> listNPCs = new List<Collider>();
    List<ComputerMessage> listComputers = new List<ComputerMessage>();

    Status playerStatus;
    Database database;

    StarterAssetsInputs input;
    bool isPlayerResurrected = false;

    struct Scenes
    {
        public static int title = 0;
        public static int newGameScene = 1;
        public static int loadScene = 2;
        public static int fatalEnding = 5;
        public static int training = 6;
    }

    // Start is called before the first frame update
    void Start()
    {        
        SetSceneCursorState();
        GetPlayerComponents();
        GetDatabase();
        SetPlayerInDatabase();
        ReadSceneData();
        RetrievePlayerResurrections();        

        void SetSceneCursorState()
        {
            if (GameObject.FindObjectOfType<UISceneManager>())
            {
                SetCursorUnlocked();

                void SetCursorUnlocked() { if (Cursor.lockState == CursorLockMode.Locked) { ChangeCursorLockStateDinamically(); } }
            }
            else
            {
                SetCursorLocked();

                void SetCursorLocked()
                {
                    if (Cursor.lockState == CursorLockMode.None)
                    {
                        ChangeCursorLockStateDinamically();
                    }
                    else
                    {
                        HideCursor();

                        void HideCursor()
                        {
                            Cursor.visible = false;
                        }
                    }
                }
            }
        }
        void GetPlayerComponents()
        {
            DetectPlayer();
            GetPlayerStatus();
            GetPlayerInputs();

            void DetectPlayer() { player = Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.player)[0].gameObject; }
            void GetPlayerStatus() { if (IsPlayer()) { playerStatus = player.GetComponent<Status>(); } }
            void GetPlayerInputs() { if (IsPlayer()) { input = player.GetComponent<StarterAssetsInputs>(); } }
        }
        void GetDatabase() { database = dataBaseObject.GetComponent<Database>(); }
        void SetPlayerInDatabase() { database.WritePlayer(player); }
        void ReadSceneData()
        {            
            if (SceneData.instance.isLoading)
            {
                DoorKeyHolder keyHolder = player.GetComponent<DoorKeyHolder>();                

                database.LoadDataBase();
                ReadPlayerData();
                database.LoadScene();
                LoadComputers();
                LoadPickUpedKeyItemsState();
                LoadKeyHoldingList();
                LoadDoorKeysList();
                LoadLockDoorsList();
                database.LoadMissions();
                database.LoadTimeLineTime();
                LoadNPCs();

                SceneData.instance.isLoading = false;

                void ReadPlayerData()
                {
                    player = database.ReadPlayer();
                }
                void LoadComputers()
                {
                    DetectComputers();
                    LoadComputersInformation();

                    void DetectComputers()
                    {
                        listComputers.AddRange(GameObject.FindObjectsOfType<ComputerMessage>());
                    }
                    void LoadComputersInformation()
                    {
                        for (int i = 0; i < listComputers.Count; i++)
                        {

                            listComputers[i] = database.LoadComputer(listComputers[i]);
                        }
                    }

                }
                void LoadPickUpedKeyItemsState()
                {
                    List<PickupKeyItem> keyItems = new List<PickupKeyItem>();
                    keyItems.AddRange(GameObject.FindObjectsOfType<PickupKeyItem>());
                    KeyItems.LoadKeyItems(database.LoadPickUpedKeyItems());

                    if (KeyItems.keyItemsList.Count > 0)
                    {
                        UIKeyItemsManager uiKeyItemManager = GameObject.FindObjectOfType<UIKeyItemsManager>();

                        foreach (PickupKeyItem keyItem in keyItems)
                        {
                            if (KeyItems.keyItemsList[keyItem.ReadKeyNameValue()]) 
                            { 
                                keyItem.gameObject.SetActive(false);
                                uiKeyItemManager.SetActive(keyItem.ReadKeyNameValue(), true);
                            }
                        }
                    }
                }
                void LoadKeyHoldingList()
                {
                    if (database.LoadDoorKeyHoldingList().Count > 0)
                    {
                        keyHolder.LoadKeyHoldingList(database.LoadDoorKeyHoldingList());
                    }
                }
                void LoadDoorKeysList()
                {
                    if (database.LoadDoorKeysObtainedFileList().Count > 0)
                    {
                        keyHolder.LoadDoorKeyObtainedList(database.LoadDoorKeysObtainedFileList());

                        List<DoorKey> doorKeysList = new List<DoorKey>();
                        doorKeysList.AddRange(GameObject.FindObjectsOfType<DoorKey>());

                        if (doorKeysList.Count > 0)
                        {
                            foreach (DoorKey doorKey in doorKeysList)
                            {
                                foreach (DoorKeyFile doorKeyFile in database.LoadDoorKeysObtainedFileList())
                                {                                    
                                    if (doorKey.key.keyColor == doorKeyFile.color)
                                    {
                                        doorKey.SetIsObtained(doorKeyFile.isObtained);
                                        doorKey.DestroyObtained();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                void LoadLockDoorsList()
                {
                    if (keyHolder)
                    {
                        if (database.LoadDoorsLockList().Count > 0)
                        {
                            List<DoorLock> doorLocksList = new List<DoorLock>();
                            doorLocksList.AddRange(GameObject.FindObjectsOfType<DoorLock>());

                            if (doorLocksList.Count > 0)
                            {
                                foreach (DoorLock doorLock in doorLocksList)
                                {
                                    foreach (DoorLockFile doorLockFile in database.LoadDoorsLockList())
                                    {
                                        if (doorLockFile.name == doorLock.gameObject.name)
                                        {
                                            keyHolder.LoadDoorLock(doorLock, doorLockFile.removeOnUse, doorLockFile.isOpened, doorLockFile.isKeyObtained);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                void LoadNPCs()
                {
                    Status npcStatus;

                    DetectNPCs();
                    LoadNPCsInformation();                    
                    ActivatingLoadedTeleportedNPCs();
                    ActivateNPCsLoaded();

                    void DetectNPCs()
                    {
                        listNPCs.Clear();
                        listNPCs.AddRange(Sensor.GetNearbyColliders(Layers.npc, transform.position, Mathf.Infinity));
                        foreach (Collider npc in listNPCs)
                        {
                            npcStatus = GetStatus(npc.gameObject);
                            if (!npcStatus.isTeleported) { npcStatus.death = true; }
                        }
                        // listNPCs.AddRange(Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.npc));
                    }
                    void LoadNPCsInformation()
                    {
                        for (int i = 0; i < listNPCs.Count; i++)
                        {
                            listNPCs[i] = database.LoadNPC(listNPCs[i]);
                        }
                    }
                    void ActivatingLoadedTeleportedNPCs()
                    {
                        for (int i = 0; i < listNPCs.Count; i++)
                        {
                            npcStatus = GetStatus(listNPCs[i].gameObject);
                            if (npcStatus.isTeleported) { ActivateTeleportedNPCs(listNPCs[i].gameObject); }
                        }
                        
                        void ActivateTeleportedNPCs(GameObject npc)
                        {
                            if (npcStatus.isTeleportingActivated)
                            {
                                if (database.IsNPCInDB(npcStatus))
                                {
                                    HealthBarManager healthBar;
                                    npc.SetActive(true);
                                    healthBar = npc.GetComponent<HealthBarManager>();
                                    healthBar.ActualizeMaxHealth(npcStatus.Max_health, npcStatus.health);
                                }
                                else
                                {
                                    npc.SetActive(false);
                                }
                            }
                            else
                            {
                                npc.SetActive(false);
                            }
                        }
                    }
                    void ActivateNPCsLoaded()
                    {
                        foreach (Collider npc in listNPCs)
                        {
                            npcStatus = GetStatus(npc.gameObject);
                            if (!npcStatus.isTeleported)
                            {
                                if (database.IsNPCInDB(npcStatus)) { npcStatus.death = false; }
                                else
                                {
                                    npc.gameObject.SetActive(false);
                                }
                            }
                        }
                    }

                    Status GetStatus(GameObject npc) { return npc.GetComponent<Status>(); }
                }
            }
            else if (SceneData.instance.isNewGame)
            {             
                NameGamer();
                SceneData.instance.isNewGame = false;

                void NameGamer()
                {
                    if (SceneData.instance.gamerName != null)
                    {
                        if (SceneData.instance.gamerName.Trim() != string.Empty)
                        {
                            playerStatus.gamerName = SceneData.instance.gamerName;                            
                        }
                    }
                }                
            }
            else
            {
                SceneData.instance.UpdatePlayerData(playerStatus);
            }
        }
        void RetrievePlayerResurrections()
        {
            if (SceneData.instance.isResurrection)
            {
                playerStatus.resurrections = SceneData.instance.resurrections;
                player.gameObject.GetComponent<PlayerLevelCanvasManager>().SetLevelByStatus(playerStatus);
                SceneData.instance.isResurrection = false;
                SceneData.instance.resurrections = 0;
            }
        }
    }

    // Update is called once per frame

    // POLYMORPHISM
    void Update()
    {
        if (IsPlayer())
        {
            if (IsPlayerRunned())
            {
                if (IsPlayerRunned()) 
                {
                    int fatalEndUI = GetFatalEndingIndex();
                    ChangeCursorLockStateDinamically();
                    LoadSceneAsynchronously(fatalEndUI, Scenes.fatalEnding) ;
                    
                    int GetFatalEndingIndex() { return Scenes.fatalEnding - 2; }
                }
            }
            else if (IsPlayerDeath())
            {
                if (isTrainingRoom)
                {
                    LoadSceneAsynchronously(Scenes.title);
                }
                else
                {
                    UploadNumberOfPlayerResurrections();
                    UploadCountingMissionRewardApplyed();
                    LoadSceneAsynchronously(Scenes.newGameScene);

                    void UploadNumberOfPlayerResurrections()
                    {
                        AddPlayerResurrections(); isPlayerResurrected = true;
                        SceneData.instance.UploadPlayerData(playerStatus);

                        void AddPlayerResurrections()
                        {
                            if (!isPlayerResurrected)
                            {
                                playerStatus.resurrections++; // Add player resurrections
                                SceneData.instance.resurrections = playerStatus.resurrections; // Up load resurrections between scenes. 
                                SceneData.instance.isResurrection = IsPlayerDeath(); // Activate resurrections when player is death.
                            }
                        }
                    }
                    void UploadCountingMissionRewardApplyed()
                    {
                        GiftCountingMission countingMission = GameObject.FindObjectOfType<GiftCountingMission>();

                        if (countingMission) { SceneData.instance.UploadCountingMissionRewardAdded(countingMission.ReadRewardApplyed()); }
                    }
                }
            }

            bool IsPlayerRunned() { return isPlayerRunned; }
            bool IsPlayerDeath()
            {
                if (IsPlayerStatus())
                {
                    return playerStatus.death;
                }
                else
                {
                    Debug.LogError("Player do not have status component, Add one to him!!!");
                    return false;
                }

                bool IsPlayerStatus() { return playerStatus != null; }
            }
        }

        CheckPausingGameWindows();

        void CheckPausingGameWindows()
        {
            bool isWindowPausing = false;

            if (IsUIMenu()) { ShowUIMenu(); }
            if (IsStatus()) { ShowUIStatus(); }

            CheckGamePaused(isWindowPausing);

            bool IsUIMenu() { return uiMenu != null; }
            bool IsStatus() { return uiStatus != null; }
            void ShowUIMenu()
            {
                uiMenu.SetActive(IsShowUIMenu());
                isWindowPausing = isWindowPausing || uiMenu.activeSelf;

                bool IsShowUIMenu()
                {
                    if (input.uiSystem)
                    {
                        input.uiSystem = false;
                        ChangeCursorLockStateDinamically();
                        return !uiMenu.activeSelf;
                    }
                    else
                    {
                        return uiMenu.activeSelf;
                    }
                }
            }
            void ShowUIStatus()
            {

                uiStatus.SetActive(IsShowStatusWindowActivated());
                uiStatus.GetComponent<UIStatusManager>().PrintInUIStatus(player, playerStatus);
                isWindowPausing = isWindowPausing || uiStatus.activeSelf;

                bool IsShowStatusWindowActivated()
                {
                    if (input.status)
                    {
                        input.status = false;
                        ChangeCursorLockStateDinamically();
                        return !uiStatus.activeSelf;
                    }
                    else
                    {
                        return uiStatus.activeSelf;
                    }
                }
            }
        }
    }

    // UI Buttons methods:
    public void LoadGame()
    {
        HideUIMenu();
        database.LoadDataBase();
        SceneData.instance.isLoading = true;
        LoadSceneAsynchronously(Scenes.loadScene, database.SceneTable.index);
        // SceneManager.LoadScene(database.SceneTable.index);
    }

    public void SaveGame()
    {
        HideUIMenu();
        database.SaveData(SceneManager.GetActiveScene().buildIndex);
    }

    // POLYMORPHISM
    public void NewGame()
    {
        if (isTrainingRoom)
        {
            LoadSceneAsynchronously(Scenes.title);
        }
        else 
        { 
            HideUIMenu();
            KeyItems.RestartKeyItems();
            SceneData.instance.isNewGame = true;
            LoadSceneAsynchronously(Scenes.newGameScene);
        }
    }

    public void Resume()
    {
        HideUIMenu();
        HideStatusWindow();
    }

    public void Title()
    {
        HideUIMenu();
        UploadPlayerStatusToHallOfHonor();
        LoadSceneAsynchronously(Scenes.title);

        void UploadPlayerStatusToHallOfHonor()
        {
            SceneData.instance.UploadPlayerData(playerStatus);
            SceneData.instance.SetExitingGame(true);
        }
    }

    public void Training()
    {
        int uiTraingRobotsFacility = Scenes.training - 3;
        LoadSceneAsynchronously(uiTraingRobotsFacility, Scenes.training);
    }

    // POLYMORPHISM
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif        
    }

    // ABSTRACTION
    public void SetPlayerRunned() { isPlayerRunned = true; }

    // Update:
    // ABSTRACTION
    bool IsPlayer() { return player != null; }

    // ABSTRACTION
    void HideUIMenu()  // Are used by Load, Save, New, Resume UIButtons to hidethe menu.
    {        
        if (uiMenu)
        {
            if (uiMenu.activeSelf)
            {
                uiMenu.SetActive(false);
                CheckGamePaused(uiMenu.activeSelf);
                ChangeCursorLockStateDinamically();
            }
        }
    }

    // ABSTRACTION
    void HideStatusWindow()  // Are used by Load, Save, New, Resume UIButtons to hidethe menu.
    {
        if (uiStatus)
        {
            
            if (uiStatus.activeSelf)
            {
                uiStatus.SetActive(false);
                CheckGamePaused(uiStatus.activeSelf);
                ChangeCursorLockStateDinamically();
            }
        }
    }

    // ABSTRACTION
    void CheckGamePaused(bool isPaused)
    {
        if (isPaused) 
        { 
            Time.timeScale = 0f; 
        } 
        else 
        { 
            Time.timeScale = 1f;
        }
    }

    // ABSTRACTION
    void ChangeCursorLockStateDinamically()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // ABSTRACTION
    // POLYMORPHISM
    void LoadSceneAsynchronously(int sceneToLoad)
    {
        AsyncSceneLoader asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
        if (asyncSceneLoader)
        {
            if (uiInterfacesToWait[sceneToLoad])
            {
                asyncSceneLoader.LoadWithIndex(uiInterfacesToWait[sceneToLoad], sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else
        {
            if (uiInterfacesToWait[sceneToLoad])
            {
                uiInterfacesToWait[sceneToLoad].SetActive(true);
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    // ABSTRACTION
    // POLYMORPHISM
    void LoadSceneAsynchronously(int sceneUI, int sceneToLoad)
    {
        AsyncSceneLoader asyncSceneLoader = GameObject.FindObjectOfType<AsyncSceneLoader>();
        if (asyncSceneLoader)
        {
            if (uiInterfacesToWait[sceneUI])
            {
                asyncSceneLoader.LoadWithIndex(uiInterfacesToWait[sceneUI], sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        else
        {
            if (uiInterfacesToWait[sceneUI])
            {
                uiInterfacesToWait[sceneUI].SetActive(true);
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
