using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Database : MonoBehaviour
{
    public ScenesTable SceneTable { get { return _scenesTable; } private set { value = _scenesTable; } }
    ScenesTable _scenesTable;

    StatusTable _statusTable;    
    GameObject _player;    
    Status _status;
    RoomList _roomList;
        
    List<string> npcNames = new List<string>();

    [Tooltip("Gift counting mission, if there is one")] public GiftCountingMission countingMission;

    // Methods:
    private void Awake()
    {
        InstantiateTables();
        GetLists();

        void InstantiateTables()
        {
            _statusTable = new StatusTable();
            _scenesTable = new ScenesTable();
            _scenesTable.room = new List<Room>();
            _scenesTable.npcs = new List<NPC>();
        }
        void GetLists()
        {
            _roomList = GameObject.FindObjectOfType<RoomList>();
        }
    }

    // Button methods:

    public void SaveData(int sceneTableIndex)
    {
        SavePlayerData();
        SaveSceneData();
                
        void SavePlayerData()
        {
            _status = _player.GetComponent<Status>();
            WriteStatusData(_player, _status, _statusTable);
            string jsonFile = JsonUtility.ToJson(_statusTable);
            File.WriteAllText(PlayerTablePath(), jsonFile);
        }
        void SaveSceneData()
        {
            ScenesTable sceneTable = new ScenesTable();
            DoorKeyHolder keyHolder;

            sceneTable.index = sceneTableIndex;
            sceneTable = ObtainActiveRooms();
            sceneTable = ObtainActiveNPCs();
            sceneTable = ObtainActiveComputers();
            sceneTable = ObtainActiveGiftCountingMissionCurrentCounting();
            sceneTable = ObtainKeyItemList();
            sceneTable = ObtainDoorKeyHoldingList();
            sceneTable = ObtainDoorKeysList();
            sceneTable = ObtainDoorLocksList();
            sceneTable = ObtainTimeLineTime();
            sceneTable = ObtainNPCCurrentPatrolPoint();

            string jsonFile = JsonUtility.ToJson(sceneTable);
            File.WriteAllText(SceneTablePath(), jsonFile);

            // Save different objects.
            ScenesTable ObtainActiveRooms()
            {
                sceneTable.room = new List<Room>();
                List<RoomsManager> rooms = new List<RoomsManager>();
                rooms.AddRange(GameObject.FindObjectsOfType<RoomsManager>());

                for (int i = 0; i < rooms.Count; i++)
                {
                    sceneTable.room.Add(new Room() { name = rooms[i].gameObject.name, state = rooms[i].gameObject.activeSelf });
                }

                return sceneTable;
            }
            ScenesTable ObtainActiveNPCs()
            {
                // Obtain npcs in current scene:
                sceneTable.npcs = new List<NPC>();
                List<Collider> npcs = new List<Collider>();
                npcs.AddRange(Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.npc));

                // Add npc name and npc status informations:
                for (int i = 0; i < npcs.Count; i++)
                {
                    sceneTable.npcs.Add(new NPC() { statusTable = new StatusTable() });

                    WriteStatusData(npcs[i].gameObject, npcs[i].GetComponent<Status>(), sceneTable.npcs[i].statusTable);

                    // If npc is in a current counting mission, save if it is counted.
                    
                    // if (countingMission)
                    // {
                    GiftCountingMissionDialogue npcCountingDialogue = npcs[i].GetComponent<GiftCountingMissionDialogue>();
                    GiftCountingMissionDialogueNew npcCountingDialogueNew = npcs[i].GetComponent<GiftCountingMissionDialogueNew>();                        


                    if (npcCountingDialogue) 
                    { 
                        sceneTable.npcs[i].counted = npcCountingDialogue.isCounted;
                    }
                    else if (npcCountingDialogueNew)
                    {
                        sceneTable.npcs[i].counted = npcCountingDialogueNew.isCounted;
                        sceneTable.npcs[i].isCountingMissionRewardApplyed = npcCountingDialogueNew.ReadCountingMissionRewardApplyed();
                    }                        
                    // }

                    // If npcs ask player to offers a key Item to them can be counted to offer a gift after a Key finded counting Mission.
                    GiftKeyFindedCountingDialogue npcKeyFindedCountingDialogue = npcs[i].GetComponent<GiftKeyFindedCountingDialogue>();

                    if (npcKeyFindedCountingDialogue)
                    {
                        sceneTable.npcs[i].counted = npcKeyFindedCountingDialogue.isCounted;
                        sceneTable.npcs[i].showGift = npcKeyFindedCountingDialogue.ReadGiftState();
                        sceneTable.npcs[i].isKeyFindedMissionRewardApplyed = npcKeyFindedCountingDialogue.ReadKeyFindedRewardApplyed();
                        sceneTable.npcs[i].isCountingMissionRewardApplyed = npcKeyFindedCountingDialogue.ReadCountingMissionRewardApplyed();
                    }


                    // If npc offer a gift after he finished talking save the gift show state.
                    GiftDialogue npcGiftDialogue = npcs[i].GetComponent<GiftDialogue>();
                    GiftDialogueNew npcGiftDialogueNew = npcs[i].GetComponent<GiftDialogueNew>();
                    Status status = npcs[i].GetComponent<Status>();
                    
                    if (npcGiftDialogue) 
                    { 
                        sceneTable.npcs[i].showGift = npcGiftDialogue.IsFinishingDialogue(); 
                    }
                    else if (npcGiftDialogueNew)
                    {
                        sceneTable.npcs[i].showGift = npcGiftDialogueNew.ReadShowGift();
                    }

                    // If npc offers a gift in a key item finded dialogue:
                    GiftKeyFindedMissionDialogue npcGiftKeyFinded = npcs[i].GetComponent<GiftKeyFindedMissionDialogue>();
                    GiftKeyFindedMissionDialogueNew npcGiftKeyFindedNew = npcs[i].GetComponent<GiftKeyFindedMissionDialogueNew>();

                    if (npcGiftKeyFinded) 
                    { 
                        sceneTable.npcs[i].showGift = npcGiftKeyFinded.ReadGiftState();                        
                    }
                    else if (npcGiftKeyFindedNew)
                    {
                        sceneTable.npcs[i].showGift = npcGiftKeyFindedNew.ReadGiftState();
                        sceneTable.npcs[i].isKeyFindedMissionRewardApplyed = npcGiftKeyFindedNew.ReadKeyFindedRewardApplyed();
                    }
                }

                return sceneTable;
            }
            ScenesTable ObtainActiveComputers()
            {
                // Obtain npcs in current scene:
                if (sceneTable.computers != null) { sceneTable.computers.Clear(); } else { sceneTable.computers = new List<Computer>(); }
                List<ComputerMessage> computers = new List<ComputerMessage>();
                computers.AddRange(GameObject.FindObjectsOfType<ComputerMessage>());

                for (int i = 0; i < computers.Count; i++)
                {
                    sceneTable.computers.Add(new Computer
                    {
                        name = computers[i].name,
                        isKeyOffered = computers[i].ReadKeyState(),
                        turns = computers[i].ReadTurns()
                    });
                }

                return sceneTable;
            }
            ScenesTable ObtainActiveGiftCountingMissionCurrentCounting()
            {
                if (countingMission) { sceneTable.countingMissionCurrentCounting = countingMission.GetCount(); }
                return sceneTable;
            }
            ScenesTable ObtainKeyItemList()
            {
                if (sceneTable.keyItemsList == null) { sceneTable.keyItemsList = new List<bool>(); }
                sceneTable.keyItemsList.AddRange(KeyItems.keyItemsList);

                return sceneTable;
            }
            ScenesTable ObtainTimeLineTime()
            {
                TimelineManager timelineManager;
                timelineManager = GameObject.FindObjectOfType<TimelineManager>();
                if (timelineManager) { sceneTable.timeLineTime = timelineManager.ReadTime(); }

                return sceneTable;
            }
            ScenesTable ObtainDoorKeyHoldingList()
            {
                sceneTable.doorKeysHoldingList = new List<Key>();
                keyHolder = GameObject.FindObjectOfType<DoorKeyHolder>();

                if (keyHolder) { sceneTable.doorKeysHoldingList.AddRange(keyHolder.ReadKeyHoldingList()); }

                return sceneTable;
            }
            ScenesTable ObtainDoorKeysList()
            {
                if (keyHolder)
                {
                    sceneTable.doorKeysList = new List<DoorKeyFile>();
                                        
                    foreach (DoorKeyFile doorKeyObtained in keyHolder.ReadDoorKeyObtainedList())
                    {
                        sceneTable.doorKeysList.Add(new DoorKeyFile { color = doorKeyObtained.color, isObtained = doorKeyObtained.isObtained });
                    }
                }
                return sceneTable;
            }
            ScenesTable ObtainDoorLocksList()
            {
                sceneTable.doorsLockList = new List<DoorLockFile>();
                List<DoorLock> doorLocks = new List<DoorLock>();
                doorLocks.AddRange(GameObject.FindObjectsOfType<DoorLock>());                

                foreach(DoorLock doorLock in doorLocks)
                {
                    // Debug.Log($"Door lock: {doorLock.gameObject.name}, key removed: {doorLock.removeKeyOnUse}, is opened: {doorLock.ReadIsOpened()}, is obtained: {doorLock.ReadIsObtained()}.");
                    sceneTable.doorsLockList.Add
                        (new DoorLockFile
                            {
                                name = doorLock.gameObject.name,                        
                                removeOnUse = doorLock.removeKeyOnUse,
                                isOpened = doorLock.ReadIsOpened(),
                                isKeyObtained = doorLock.ReadIsObtained()
                            }
                        );
                }
                
                return sceneTable;
            }
            ScenesTable ObtainNPCCurrentPatrolPoint()
            {
                List<Collider> npcs = new List<Collider>();
                npcs.AddRange(Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.npc));

                for (int i = 0; i < npcs.Count; i++)
                {
                    // Set player next patrolpoint
                    Patrol patrol = npcs[i].gameObject.GetComponent<Patrol>();
                    if (patrol) { sceneTable.npcs[i].patrolPoint = patrol.ReadPatrolPoint(); }
                }

                return sceneTable;
            }
        }
    }

    public void LoadDataBase()
    {        
        LoadSceneTable();
        LoadPlayerTable();

        void LoadSceneTable()
        {
            string tablePath = SceneTablePath();

            if (File.Exists(tablePath))
            {
                string jsonFile = File.ReadAllText(SceneTablePath());
                _scenesTable = JsonUtility.FromJson<ScenesTable>(jsonFile);
            }
        }
        void LoadPlayerTable()
        {
            string tablePath = PlayerTablePath();

            if (File.Exists(tablePath))
            {
                string jsonFile = File.ReadAllText(PlayerTablePath());
                _statusTable = JsonUtility.FromJson<StatusTable>(jsonFile);
                ReadStatusData(_player, _status, _statusTable);
                LoadPlayerHealthbar();
                LoadPlayerLevelCanvas();

                void LoadPlayerHealthbar()
                {
                    HealthBarManager playerHealthbar = _player.GetComponent<HealthBarManager>();
                    if (playerHealthbar)
                    {                        
                        playerHealthbar.ActualizeMaxHealth(_status.max_health, _status.health);                        
                    }
                }
                void LoadPlayerLevelCanvas()
                {
                    PlayerLevelCanvasManager levelCanvas = _player.GetComponent<PlayerLevelCanvasManager>();
                    if (levelCanvas)
                    {
                        levelCanvas.SetLevelByStatus(_status);
                    }
                }
            }
        }
    }

    public void LoadScene()
    {
        ActiveLoadedRooms();

        void ActiveLoadedRooms()
        {
            _roomList.HideRooms();
            foreach (Room room in _scenesTable.room)
            {
                _roomList.ActivateRoom(_roomList.RoomIndex(room.name));
            }

        }
    }

    public bool IsNPCInDB(Status npcStatus)
    {
        bool finded = false;
        foreach (NPC npc in _scenesTable.npcs) 
        {
            if (npc.statusTable.actorName == npcStatus.actorName) { finded = true; break; }
        }

        return finded;
    }
    public Collider LoadNPC(Collider npc)
    {        
        npc = LoadNPCData(npc);

        return npc;

        Collider LoadNPCData(Collider npcCollider)
        {            
            _status = npcCollider.GetComponent<Status>();
            int npcIndex = GetNPCIndex(_status.actorName);
                        
            if (npcIndex >= 0)
            {
                ReadStatusData(npcCollider.gameObject, _status, _scenesTable.npcs[npcIndex].statusTable);
                CheckGiftOffer(npcCollider, npcIndex);
                CheckGiftKeyFinded(npcCollider, npcIndex);
                CheckCountedInMission(npcCollider, npcIndex);
                SetNextPatrolPoint(npcCollider, npcIndex);

                void CheckGiftOffer(Collider npcCollider, int npcIndex)
                {
                    if (npcCollider)
                    {
                        GiftDialogue npcDialogue = npcCollider.gameObject.GetComponent<GiftDialogue>();
                        GiftDialogueNew npcDialogueNew = npcCollider.gameObject.GetComponent<GiftDialogueNew>();

                        if (npcDialogue) 
                        { 
                            npcDialogue.SetShowGift(_scenesTable.npcs[npcIndex].showGift); 
                        }
                        else if (npcDialogueNew)
                        {
                            npcDialogueNew.SetShowGift(_scenesTable.npcs[npcIndex].showGift);
                        }
                    }
                }
                void CheckGiftKeyFinded(Collider npcCollider, int npcIndex)
                {
                    if (npcCollider)
                    {
                        GiftKeyFindedMissionDialogue npcDialogue = npcCollider.gameObject.GetComponent<GiftKeyFindedMissionDialogue>();
                        GiftKeyFindedMissionDialogueNew npcDialogueNew = npcCollider.gameObject.GetComponent<GiftKeyFindedMissionDialogueNew>();                        

                        if (npcDialogue)
                        {
                            int keyIndex = npcDialogue.KeyFindedIndex();
                            npcDialogue.SetGiftState(KeyItems.keyItemsList[keyIndex], _scenesTable.npcs[npcIndex].showGift);
                        }
                        else if (npcDialogueNew)
                        {
                            int keyIndex = npcDialogueNew.KeyFindedIndex();
                            npcDialogueNew.SetGiftState(_scenesTable.npcs[npcIndex].showGift);
                            npcDialogueNew.SetKeyFindedRewardApplyed(_scenesTable.npcs[npcIndex].isKeyFindedMissionRewardApplyed);
                        }
                    }
                }
                void CheckCountedInMission(Collider npcCollidr, int npcIndex)
                {
                    GiftCountingMissionDialogue npcCountingMissionDialogue;
                    GiftCountingMissionDialogueNew npcCountingMissionDialogueNew;
                    GiftKeyFindedCountingDialogue npcKeyFindedCountingMissionDialogue;

                    npcCountingMissionDialogue = npcCollidr.gameObject.GetComponent<GiftCountingMissionDialogue>();
                    npcCountingMissionDialogueNew = npcCollidr.gameObject.GetComponent<GiftCountingMissionDialogueNew>();
                    npcKeyFindedCountingMissionDialogue = npcCollidr.gameObject.GetComponent<GiftKeyFindedCountingDialogue>();

                    if (npcCountingMissionDialogue) 
                    { 
                        npcCountingMissionDialogue.isCounted = _scenesTable.npcs[npcIndex].counted; 
                    }
                    else if (npcCountingMissionDialogueNew)
                    {
                        npcCountingMissionDialogueNew.isCounted = _scenesTable.npcs[npcIndex].counted;
                        npcCountingMissionDialogueNew.SetCountingMissionRewardApplyed(_scenesTable.npcs[npcIndex].isCountingMissionRewardApplyed);
                    }
                    else if (npcKeyFindedCountingMissionDialogue)
                    {                        
                        npcKeyFindedCountingMissionDialogue.SetCounted(_scenesTable.npcs[npcIndex].counted);
                        npcKeyFindedCountingMissionDialogue.SetGiftState(_scenesTable.npcs[npcIndex].showGift);
                        npcKeyFindedCountingMissionDialogue.SetKeyFindedRewardApplyed(_scenesTable.npcs[npcIndex].isKeyFindedMissionRewardApplyed);
                        npcKeyFindedCountingMissionDialogue.SetCountingMissionRewardAppyed(_scenesTable.npcs[npcIndex].isCountingMissionRewardApplyed);
                    }
                }
                void SetNextPatrolPoint(Collider npcCollider, int npcIndex)
                {
                    // Save npc next patrolpoint
                    Patrol Patrol = npcCollider.GetComponent<Patrol>();
                    if (Patrol) { Patrol.SetPatrolPoint(_scenesTable.npcs[npcIndex].patrolPoint); }
                }
            }

            return npcCollider;

            int GetNPCIndex(string npcName)
            {
                foreach (NPC npc in _scenesTable.npcs)
                {
                    npcNames.Add(npc.statusTable.actorName);
                }

                return npcNames.IndexOf(npcName);
            }
        }
    }

    public ComputerMessage LoadComputer(ComputerMessage computer)
    {
        computer = LoadComputerData(computer);

        return computer;

        ComputerMessage LoadComputerData(ComputerMessage computerMessage)
        {
            int index = GetComputerIndex(computerMessage.name);
            int turns = _scenesTable.computers[index].turns;
            computerMessage.SetKeyState(_scenesTable.computers[index].isKeyOffered, turns);

            return computerMessage;

            int GetComputerIndex(string name)
            {
                List<string> computerName = new List<string>();
                foreach (Computer computerItem in _scenesTable.computers)
                {
                    computerName.Add(computerItem.name);
                }

                int index = computerName.IndexOf(name);
                return index;
            }
        }
    }

    public void LoadMissions() 
    { 
        if (countingMission) { countingMission.SetCount(SceneTable.countingMissionCurrentCounting); } 
    }

    public void LoadTimeLineTime()
    {
        TimelineManager timelineManager;
        timelineManager = GameObject.FindObjectOfType<TimelineManager>();
        if (timelineManager) { timelineManager.SetTime(SceneTable.timeLineTime); }
    }

    public List<bool> LoadPickUpedKeyItems() 
    { 
        return SceneTable.keyItemsList;
    }

    public List<Key> LoadDoorKeyHoldingList()
    {
        return _scenesTable.doorKeysHoldingList;        
    }    

    public List<DoorKeyFile> LoadDoorKeysObtainedFileList()
    {
        return SceneTable.doorKeysList;
    }

    public List<DoorLockFile> LoadDoorsLockList()
    {
        return SceneTable.doorsLockList;
    }

    public void WritePlayer(GameObject player)
    {
        _player = player;
        GetPlayerComponents();

        void GetPlayerComponents()
        {
            _player = Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.player)[0].gameObject;
            _status = _player.GetComponent<Status>();
        }
    }

    public GameObject ReadPlayer()
    {
        return _player;
    }

    // POLYMORPHISM
    // ABSTRACTION
    // LoadData Player data:
    void ReadStatusData(GameObject sceneGameObject, Status gameObjectStatus, StatusTable statusTable)
    {
        // Player position:
        sceneGameObject.SetActive(false);
        sceneGameObject.transform.position = statusTable.position;
        sceneGameObject.transform.rotation = statusTable.rotation;
        sceneGameObject.SetActive(true);

        // Gamer information:
        gameObjectStatus.gamerName = statusTable.gamerName;
        gameObjectStatus.enemiesKilled = statusTable.enemiesKilled;

        // General information:
        gameObjectStatus.actorName = statusTable.actorName;
        gameObjectStatus.description = statusTable.description;

        // Player level:
        gameObjectStatus.level = statusTable.level;
        gameObjectStatus.maxLevel = statusTable.maxLevel;
        gameObjectStatus.experience = statusTable.experience;
        gameObjectStatus.experienceNextLevel = statusTable.experienceNextLevel;
        gameObjectStatus.experienceGrouthRate = statusTable.experienceGrouthRate;

        // Player basic attributes:
        gameObjectStatus.resurrections = statusTable.resurrections;
        gameObjectStatus.health = statusTable.health;
        gameObjectStatus.max_health = statusTable.max_health;
        gameObjectStatus.mana = statusTable.mana;
        gameObjectStatus.max_mana = statusTable.max_mana;

        // Player general attributes:
        gameObjectStatus.strength = statusTable.strength;
        gameObjectStatus.endurance = statusTable.endurance;
        gameObjectStatus.dexterity = statusTable.dexterity;
        gameObjectStatus.wisdom = statusTable.wisdom;
        gameObjectStatus.faith = statusTable.faith;
        gameObjectStatus.resistance = statusTable.resistance;

        // Player bad states:
        gameObjectStatus.death = statusTable.death;
        gameObjectStatus.sleep = statusTable.sleep;
        gameObjectStatus.poison = statusTable.poison;
        gameObjectStatus.hungry = statusTable.hungry;
        gameObjectStatus.blood = statusTable.blood;
        gameObjectStatus.burn = statusTable.burn;
        gameObjectStatus.slow = statusTable.slow;
        gameObjectStatus.stop = statusTable.stop;
        gameObjectStatus.confuse = statusTable.confuse;
        gameObjectStatus.berserk = statusTable.berserk;
        gameObjectStatus.posess = statusTable.posess;

        // PLayer good states:
        gameObjectStatus.protect = statusTable.protect;
        gameObjectStatus.resist = statusTable.resist;
        gameObjectStatus.reflect = statusTable.reflect;
        gameObjectStatus.bost = statusTable.bost;
        gameObjectStatus.magicBost = statusTable.magicBost;
        gameObjectStatus.quick = statusTable.quick;

        // Player reward to NPCs if he is death:
        gameObjectStatus.experienceReward = statusTable.experienceReward;

        // Player Alignment:
        gameObjectStatus.goodAlignment = statusTable.goodAlignment;

        // NPC activated by teleporter:
        gameObjectStatus.isTeleported = statusTable.isTeleported;
        gameObjectStatus.isTeleportingActivated = statusTable.isTeleportingActivated;
    }

    // POLYMORPHISM
    // ABSTRACTION
    // SavePlayerData:
    void WriteStatusData(GameObject sceneGameObject, Status gameObjectStatus, StatusTable statusTable)
    {        
        // position:
        sceneGameObject.SetActive(false);
        statusTable.position = sceneGameObject.transform.position;
        statusTable.rotation = sceneGameObject.transform.rotation;
        sceneGameObject.SetActive(true);

        // Gamer information:
        statusTable.gamerName = gameObjectStatus.gamerName;
        statusTable.enemiesKilled = gameObjectStatus.enemiesKilled;

        // General information:
        statusTable.actorName = gameObjectStatus.actorName;
        statusTable.description = gameObjectStatus.description;        

        // Player level:
        statusTable.level = gameObjectStatus.level;
        statusTable.maxLevel = gameObjectStatus.maxLevel;
        statusTable.experience = gameObjectStatus.experience;
        statusTable.experienceNextLevel = gameObjectStatus.experienceNextLevel;
        statusTable.experienceGrouthRate = gameObjectStatus.experienceGrouthRate;

        // Player basic attributes:
        statusTable.resurrections = gameObjectStatus.resurrections;
        statusTable.health = gameObjectStatus.health;
        statusTable.max_health = gameObjectStatus.max_health;
        statusTable.mana = gameObjectStatus.mana;
        statusTable.max_mana = gameObjectStatus.max_mana;

        // Player general attributes:
        statusTable.strength = gameObjectStatus.strength;
        statusTable.endurance = gameObjectStatus.endurance;
        statusTable.dexterity = gameObjectStatus.dexterity;
        statusTable.wisdom = gameObjectStatus.wisdom;
        statusTable.faith = gameObjectStatus.faith;
        statusTable.resistance = gameObjectStatus.resistance;

        // Player bad states:
        statusTable.death = gameObjectStatus.death;
        statusTable.sleep = gameObjectStatus.sleep;
        statusTable.poison = gameObjectStatus.poison;
        statusTable.hungry = gameObjectStatus.hungry;
        statusTable.blood = gameObjectStatus.blood;
        statusTable.burn = gameObjectStatus.burn;
        statusTable.slow = gameObjectStatus.slow;
        statusTable.stop = gameObjectStatus.stop;
        statusTable.confuse = gameObjectStatus.confuse;
        statusTable.berserk = gameObjectStatus.berserk;
        statusTable.posess = gameObjectStatus.posess;

        // Player good states:
        statusTable.protect= gameObjectStatus.protect;
        statusTable.resist = gameObjectStatus.resist;
        statusTable.reflect = gameObjectStatus.reflect;
        statusTable.bost = gameObjectStatus.bost;
        statusTable.magicBost= gameObjectStatus.magicBost;
        statusTable.quick = gameObjectStatus.quick;

        // Player reward to NPCs if he is death
        statusTable.experienceReward = gameObjectStatus.experienceReward;

        // Player Alignment:
        statusTable.goodAlignment = gameObjectStatus.goodAlignment;

        // NPC activate by teleporter:
        statusTable.isTeleported = gameObjectStatus.isTeleported;
        statusTable.isTeleportingActivated = gameObjectStatus.isTeleportingActivated;
    }

    // SaveSceneData:
    // ABSTRACTION
    string PlayerTablePath() { return $"{ Application.persistentDataPath}/PlayerTable.json"; }

    // ABSTRACTION
    string SceneTablePath() { return $"{ Application.persistentDataPath}/ScenesTable.json"; }
}
