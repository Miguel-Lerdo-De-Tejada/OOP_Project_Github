using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public abstract class Pickup : MonoBehaviour
{
    // This is the base class of any other child classes which mean to be pickuped for the player, like heal, poison, stop, strenght enhance and so on.
    [SerializeField, Tooltip("Pickup detection range"), Range(1, 5)] int detectionRange;
    [SerializeField, Tooltip("Play a particle when you pickup the item")] ParticleSystem particle;
    [SerializeField, Tooltip("Seconds to desapear the pickup")] float waitToDisapear=2f;
    protected Status playerStatus;
    protected HealthBarManager playerHealthBar;
    // StarterAssetsInputs playerInput;
    // bool IsParticlePlayed = false;

    void Update()
    {
        DetectPlayer();
    }

    // Destroy the pickup and play a particle when the pick up meets the player collider.
    void DetectPlayer()
    {
        if (IsPlayer())
        {
            GetPlayerComponents();

            if (IsParticle()) { if (!particle.isPlaying) { particle.Play(); } }
            OnAction();
            StartCoroutine(DesapearPeakup());
        }
    }

    bool IsPlayer() { return Physics.CheckSphere(transform.position, detectionRange, Layers.player); }

    bool IsParticle() { return particle != null; }

    // Get the status of the player to use in the child classes.
    void GetPlayerComponents()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRange, Layers.player);
        playerStatus = players[0].GetComponent<Status>();
        playerHealthBar = players[0].GetComponent<HealthBarManager>();
        // playerInput = players[0].GetComponent<StarterAssetsInputs>();
        players = null;
    }

    IEnumerator DesapearPeakup()
    {
        yield return new WaitForSeconds(waitToDisapear);
        
        Destroy(gameObject);
    }

    // Write the effect of the pickup in the child classes, if the player is healed or harmed, is poisoned or stopped, is augmented in strenght, or if is
    // a Key item for another action.

    protected abstract void OnAction();
}
