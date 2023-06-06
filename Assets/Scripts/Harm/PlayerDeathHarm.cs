using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerDeathHarm : Harm
{
    [Header("Destroy player")]
    [SerializeField, Tooltip("Seconds to wait to kill the player"), Range(0.1f, 3.0f)] float secondsToKillPlayer = 0.1f;
    [SerializeField, Tooltip("Player death point")] Transform deathPoint;

    protected override void OnInstantiateHitParticles(Collision target)
    {        
        
    }

    protected override void OnDamageTarget(Collider target)
    {
        GameObject player = target.gameObject;

        if (IsPlayerLayer())
        {
            InstantiateExplotion();
            DesapearPlayer();
            StartCoroutine(KillPlayer(player.gameObject));
            
            void InstantiateExplotion() { Instantiate(vfxHitGreen, deathPoint.position, Quaternion.identity); }
            void DesapearPlayer() { player.SetActive(false); }
        }

        bool IsPlayerLayer() { return Layers.ReadLayer(player.layer) == Layers.player; }
                
    }

    IEnumerator KillPlayer(GameObject player)
    {
        yield return new WaitForSeconds(secondsToKillPlayer);
        Status playerStatus;
        HealthBarManager playerHealthBar;

        GetPlayerStatusComponents();
        KillPlayer();
        ActualizePlayerHealthBar();

        void ActualizePlayerHealthBar() { playerHealthBar.SetHealth(playerStatus.health); }
        void KillPlayer() 
        {
            playerStatus.health = 0;
            playerStatus.death = true;
        }
        void GetPlayerStatusComponents()
        {
            playerStatus = player.GetComponent<Status>();
            playerHealthBar = player.GetComponent<HealthBarManager>();
        }
    }
}
