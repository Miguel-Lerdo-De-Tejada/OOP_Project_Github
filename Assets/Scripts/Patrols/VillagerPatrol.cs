using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerPatrol : Patrol
{
    // HINERITANCE
    /* VillagerPatrol inHerits from Patrol. 
     * You do not need to use the start and update or any other Monobehaviur method, becuase the base clase
    Already do this. You only need to define the behaviour of your npc when detects the player in his detection range.

    // This class uses the parent Patrol class npc object instance and c_stopSpeed and currentSpeed parent variables.
    // This class uses the parent Patrol class Animate() and AsignNextDestination() methods,

    /*
    npc is a NavMeshAgent wich contol the Villager patrol.
    c_stopSpeed is a constant wich represent zero speed to stop the villager.
    currentSpeed represent the speed of the villager before his is stopped, used to reasign his speed for the next destination when the Villager 
        do not detect the player.

    Animate() method is used to animate the villager when retakes his patrol when do not detect the player.
    AsignNextDestination() method is used to reasign the villager patrol, when the player is not detected.
    */

    protected override void OnPlayerDetectionEnter()
    {
        StopVillager();
        if (!isInanimated) { LookAtPlayer(); }
    }

    protected override void OnPlayerDetectionExit()
    {
        SetVillagerNextDestination();
    }

    void StopVillager()
    {
        npc.isStopped = true;
        if (npc.speed != c_stopSpeed)
        {            
            currentSpeed = npc.speed;
            npc.speed = c_stopSpeed;
        }
    }

    void SetVillagerNextDestination()
    {
        if (npc.isStopped)
        {
            npc.isStopped = false;
            npc.speed = currentSpeed;
            Animate();
            npc.SetDestination(npc.destination);
        }
    }
}
