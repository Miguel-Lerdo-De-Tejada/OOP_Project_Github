using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportNew : TeleportExp
{
    // This Class inherits from TeleportExp and intends to appear and desappear objects after or before the player is teletransported.
    // Use The virtual InitializeTeleporting method to initialize things in the game beggining.
    // Use The virtual AfterTeleporting method, to do Things like appear things after the player reapear in the new teleported point.
    // Use the virtual BeforeTeleporting method to do things like deactivate things before the player is teletransported.

    [Space(3), Header("NPCs, Enemies and friends activation/deactivation."),Space(3)]
    [SerializeField, Tooltip("Set NPCs that you want to activate when player has been teleported.")]
    List<GameObject> activateNPCs = new List<GameObject>();
    [SerializeField, Tooltip("Set NPCs that you want to deactivate when player has been teleported.")]
    List<GameObject> deactivateNPCs = new List<GameObject>();
    [Header("User Interface")]
    [SerializeField, Tooltip("NPC Descriptor to hide between scenarios")] GameObject descriptor;    

    Status npcStatus;

    protected override void InitializeTeleporting()
    {        
        if (activateNPCs.Count > 0)
        {            
            foreach (GameObject npc in activateNPCs)
            {
                ActivateNPC(npc);
            }
        }

        void ActivateNPC(GameObject npc)
        {
            GetStatusComponentNPC(npc);
            SetNPCActivation();
            
            void SetNPCActivation()
            {
                if (npcStatus) 
                {
                    if (npcStatus.isTeleported) { npc.SetActive(npcStatus.isTeleportingActivated); }                    
                }
            }
        }
    }
    protected override void AfterTeleporting() 
    { 
        if (activateNPCs.Count > 0)
        {
            foreach (GameObject npc in activateNPCs)
            {
                ActivateNPC(npc);
            }
        }

        void ActivateNPC(GameObject npc)
        {
            npc.SetActive(true);
            GetStatusComponentNPC(npc);

                            
            if (npcStatus) 
            {
                if (npcStatus.isTeleported) { npcStatus.isTeleportingActivated = true; }
                else { npc.SetActive(false); }
            }
        }
    }

    protected override void BeforeTeleporting() 
    {
        DeactivateDescriptor();
        if (deactivateNPCs.Count > 0)
        {
            foreach (GameObject npc in deactivateNPCs)
            {
                DeactivateNPC(npc);
            }
        }

        void DeactivateDescriptor() { if (descriptor.activeSelf) { descriptor.SetActive(false); } }
        void DeactivateNPC(GameObject npc)
        {            
            GetStatusComponentNPC(npc);
            if (npcStatus) 
            {
                npcStatus.isTeleportingActivated = false;
                if (!npcStatus.isTeleported) { npcStatus.isTeleported = true; }
            }
            npc.SetActive(false);
        }
    }

    void GetStatusComponentNPC(GameObject npc)
    {        
        if (npc) { npcStatus = npc.GetComponent<Status>(); }        
    }
}
