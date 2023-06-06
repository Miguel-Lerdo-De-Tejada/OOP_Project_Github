using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerPatrolExp : PatrolExp
{
    [Space(3), Header("Player detection"), Space(3)]
    [Tooltip("PlayerDetection range."), Range(1, 3)]
    public float playerDetectionRange;
    [SerializeField, Tooltip("If NPC is inanimated do not look at player.")] bool isInanimated;

    [Space(6), Header("Villager data"),Space(3)]
    [SerializeField, Tooltip("Villager speed"), Range(0, 6)]
    float villagerSpeed;    
    
    // Villager patrol Inherits from patrol, use this components in NPCs which are not enemies.

    protected override void ExtendAwake()
    {
        InitializeAnimator();
    }

    protected override void ExtendStart()
    {
        InitializeCurrentSpeed();

        void InitializeCurrentSpeed() { SetNPCSpeed(villagerSpeed); }
    }

    protected override void ExtendUpdate()
    {        
        MoveNPCAnimated();        
    }

    void InitializeAnimator()
    {
        animator = GetComponent<Animator>();
        if (animator && isPatrolling) { animator.Play(AnimStates.moving); }
    }

    void MoveNPCAnimated()
    {
        if (isPatrolling)
        {
            if (isWaiting)
            {
                SetNPCStopped();                
            }
            else
            {
                SetNPCMoving();
            }
            
            SetNPCSpeedAnimation();
        }
        else
        {
            StopNPC();
            if (IsPlayerDetected()) 
            { 
                if (!isInanimated) { LookAtPlayer(); }
                ShowNPCDescriptor();
            }
            else
            {
                HideNPCDescriptor();
            }

            SetNPCSpeedAnimation();
        }
    }

    void SetNPCMoving()
    {
        if (IsPlayerDetected())
        {
            StopNPC();
            LookAtPlayer();
            ShowNPCDescriptor();
        }
        else
        {
            LookAtPatrolPoint();
            MoveNPC();
            HideNPCDescriptor();
        }
    }

    void SetNPCStopped()
    {
        StopNPC();
        if (IsPlayerDetected()) { LookAtPlayer(); }
    }

    bool IsPlayerDetected() { return Sensor.Detect(Layers.player, transform.position, playerDetectionRange); }

    void LookAtPlayer()
    {
        List<Collider> player = Sensor.GetNearbyColliders(Layers.player, transform.position, playerDetectionRange);

        transform.LookAt(player[0].transform.position);
    }

    void SetNPCSpeedAnimation() { if (animator) { animator.SetFloat(AnimParam.speed, agent.speed); }
    }
}
