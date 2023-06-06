using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRobotAgresivePatrol : Patrol
{
    /*  
    SphereRobotPatrolAgresive inHerits from Patrol. 
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
    */

    // Animation parameters:
    struct AnimState
    {
        public static string walk = "Walk_Anim";
        public static string roll = "Roll_Anim";        
    }

    // Speed parameters:
    [SerializeField, Tooltip("Set the robot roll speed"), Range(0f, 20f)] float rollSpeed=10f;
    [SerializeField, Tooltip("Roll speed delay time"), Range(0f, 10f)] float speedDelay = 5f;
    struct RobotSpeed
    {
        public static float walkSpeed;
        public static float rollSpeed;
        public static float stopSpeed;
    }

    protected override void OnInitializeNPC()
    {
        AssignRobotSpeeds();
        StopSphereRobot();
        StartCoroutine(SetRobotSpeed(RobotSpeed.walkSpeed, true));

        void AssignRobotSpeeds()
        {
            RobotSpeed.walkSpeed = npc.speed;
            RobotSpeed.rollSpeed = rollSpeed;
            RobotSpeed.stopSpeed = 0;
        }
        void StopSphereRobot() { npc.speed = RobotSpeed.stopSpeed; }
    }

    protected override void OnChasingPlayer()
    {
        OnPlayerDetectionEnter();
    }

    protected override void OnPlayerDetectionEnter()
    { 
        SetWalkAnimation(false);
        FollowPlayer();
        StartCoroutine(SetRobotSpeed(RobotSpeed.rollSpeed, false));

        void FollowPlayer() { npc.SetDestination(playerPosition); }
    }

    protected override void OnPlayerDetectionExit() 
    {
        SetWalkAnimation(true);
        AsignNextDestination();
        StartCoroutine(SetRobotSpeed(RobotSpeed.walkSpeed, false));
    }

    void SetWalkAnimation(bool isActivated) { animator.SetBool(AnimState.walk, isActivated); animator.SetBool(AnimState.roll, !isActivated); }

    IEnumerator SetRobotSpeed(float speed, bool isInitializeNPC)
    {
        yield return new WaitForSeconds(speedDelay);
        npc.speed = speed;
        if (isInitializeNPC) { SetWalkAnimation(true); }
    }
}
