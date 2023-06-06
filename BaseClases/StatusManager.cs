using System.Collections;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [Header("Death effect:")]
    [SerializeField, Tooltip("Effect to death")]                            GameObject deathEffect;
    [SerializeField, Tooltip("Wait for death effect"),Range(0.01f,10.0f)]   float timeWait;
    [SerializeField, Tooltip("Destroy point")]                              Transform destroyPoint;

    [Header("Reaper NPC")]
    [SerializeField, Tooltip("Reapear point")]                              Transform reapearPoint;    

    Status status;
    HealthBarManager healthBarManager;
    Patrol patrolDescriptor;    

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    // Update is called once per frame
    void Update()
    {        
        if (status != null)
        {
            if (status.death) { KillNPC(); }
        }
        else
        {
            Debug.LogError("Attach a Status component to NPC");
        }
    }

    void GetComponents()
    {
        status = GetComponent<Status>();
        healthBarManager = GetComponent<HealthBarManager>();
        patrolDescriptor = GetComponent<Patrol>();
    }

    void KillNPC()
    {        
        Rewards playerRewad = GetComponent<Rewards>();
        playerRewad.CheckNpcDeath();
        CheckTeleported();
        StartCoroutine(DesapearNPC());

        if (deathEffect != null) { Instantiate(deathEffect, destroyPoint.position, destroyPoint.rotation); } else { Debug.LogError("Not death effect attached"); }

        void CheckTeleported() 
        {
            if (status.isTeleported) { status.isTeleportingActivated = status.isTeleportingActivated ? false : status.isTeleportingActivated; }
        }
    }

    IEnumerator DesapearNPC()
    {
        yield return new WaitForSeconds(timeWait);
        gameObject.SetActive(false);
        patrolDescriptor.HideNPCDescriptor();
        if (reapearPoint) { ReapearNPC(); }

        void ReapearNPC()
        {
            Rewards playerRewards = GetComponent<Rewards>();
            status.death = false;
            status.health = status.max_health;
            healthBarManager.SetHealth(status.health);
            gameObject.transform.position = reapearPoint.position;
            gameObject.SetActive(true);
            playerRewards.InitializeAddReward();

        }
    }
}
