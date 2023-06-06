using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : Patrol
{
    // HINERITANCE
    /* EnemyPatrol inHerits from Patrol. 
    You do not need to use the start and update or any other Monobehaviur method, becuase the base clase
    Already do this. You only need to define the behaviour of your enemy when meets the player in his detection range and in his attack range and 
    when the player goes out of the npc detection range.*/
    
    // This class uses the parent Patrol class npc object instance and .
    // AsignNextDestination() methods,

    /*
    This Class Uses the next events:
    GetComponents it is trigger on start Unity event and initialize extra components for this EnemyPatrol in particular.
    OnPlayerDetectionEnter event when the enemy detects the player.
    OnPlayerDetectionExit event when the enemy stops detecting the player.

    Special components in code:
    npc is a NavMeshAgent wich contol the Enemy patrol.
    detectionDistance is used to detect player distance in the Enemy detection range.
    playerPosition is used to set the enamy destination to the player position, playerPosition needs a 0.5 offset in y axis to he can be detected.
    currentSpeed represent the speed of the villager before his is stopped, used to reasign his speed for the next destination when the Villager 
    do not detect the player.
    
    AsignNextDestination() method is used to reasign the villager patrol, when the player is not detected.

    This class implements the enemy shoot detection to shoot the player at a certain position range.
    This Class uses the FireBullets class in the shoot instance to shoot the bullets.
    */

    [Header("Patrol shoot configuration")]
    [SerializeField, Tooltip("Set the diferent fire bullet components of the Mech robot here.")] List<FireBullets> projectiles = new List<FireBullets>();
    [SerializeField, Tooltip("Shoot Distance"),Range(c_minShootDistance,1000f)] float shootDistance;
    [SerializeField, Tooltip("Enemy vertical detection offset")] float verticalDetectOffset; // Use this offset if the enemy flyes;
    const float c_minShootDistance = 1f;    

    protected override void GetComponents()
    {
        base.GetComponents();        
    }

    protected override void OnPlayerDetectionEnter()
    {
        FollowPlayer();
        IsShoot();
    }

    protected override void OnPlayerDetectionExit()
    {
        AsignNextDestination();
        foreach (FireBullets projectil in projectiles) { projectil.isFired = false; }
    }

    void FollowPlayer()
    {
        Vector3 followPlayer = playerPosition + new Vector3(0, verticalDetectOffset, 0);
        npc.SetDestination(followPlayer);
    }

    void IsShoot()
    {
        shootDistance = Mathf.Clamp(shootDistance, c_minShootDistance, detectionDistance);
        bool isShoot = DetectingPlayer(shootDistance);
        foreach (FireBullets projectil in projectiles) { projectil.isFired = isShoot ? true : false; }        
    }
}