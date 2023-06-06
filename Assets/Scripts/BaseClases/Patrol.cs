using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Linq;

public class Patrol : MonoBehaviour
{
    /* Patrol:
     * Is the base clase to assign a npc a patrol in the game field and a behaviour when he detects the player or when he exits the player detection range in 
     * his patrol. 
     * The child classes only need to asign a npc behaviour code when the npc detects the player, and when the npc exits the player detection range.
     * The npc could be a Villager, an Enemy or a friend who joins to your group.
     * In the child classes you do not need to use MonoBehaviour methods, because this class uses the required once (Start and Update).
     * If you need to obtain more componentes of your npc, redefine the virtual GetComponents method in your child class leaving the base.GetComponents in side 
     * the GetComponents Method body.
     * In your child class use:
     * npc NavMeshAgent to obtain its attributes like, speed, isStopped, etcetera...
     * currentSpeed to asign and reasigne the npc.speed before he is stopped and when npc returns to his patrol.
     * c_StopSpeed to asign the stop speed when player is stopped.
     * detectionDistance to detect the player in certain area radious.
     * playerPosition to obtain the current player position.
     * Tags.player, to obtain the tag of the player.
     */

    // Editor variables:
    // Patrol Configuration:
    [Header("Patrol config:")]
    [SerializeField,Tooltip("AI Patrol points")] List<Transform> patrolPoints = new List<Transform>();
    int point = c_inicialPoint;
    protected float currentSpeed;
    protected NavMeshAgent npc;
    Status npcStatus;
    string npcDetectingPlayerName = string.Empty;

    //Patrol configuration constants:
    const int c_inicialPoint = 0;
    protected const int c_stopSpeed = 0;

    // Player detection configuration:
    [Header("Player detection")]
    [SerializeField, Tooltip("Distance"), Range(1f, 1000f)] protected float detectionDistance = 3f;
    [SerializeField, Tooltip("If NPC is inanimated, it will not turn to look the player.")] protected bool isInanimated;
    [SerializeField, Tooltip("NPC descriptor UI")] GameObject npcDescriptor;
    protected Vector3 playerPosition;
    bool isPlayerDetected = false;

    // Structures for detection:
    protected struct Tags
    {
        public static string player = "Player";
    }


    // NPC animation:
    protected Animator animator;

    // Structures for animantions:
    struct AnimationName
    {
        public static string speed = "Speed";
    }

    // Unity Event methods:

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
        OnInitializeNPC();
        AsignNextDestination();
    }

    // Update is called once per frame
    void Update()
    {
        AsignDestination();
        DetectPlayer();
        Animate();
    }

    // Methods:
    void AsignDestination()
    {        
        if (!IsMoving())
        {
            if (IsLastPatrolPoint()) { AsignInitialPatrolPoint(); }
            
            AsignNextDestination();
            AddPatrolPoint();
        }        
    }

    bool IsMoving() { return npc.remainingDistance > npc.stoppingDistance; }

    bool IsLastPatrolPoint() { return point >= patrolPoints.Count; }

    void AsignInitialPatrolPoint() { point = c_inicialPoint; }

    void AddPatrolPoint() { point++; }

    protected void Animate()
    {
        if (animator != null) { animator.SetFloat(AnimationName.speed, npc.speed); }
    }

    protected void AsignNextDestination()
    {
        if (patrolPoints.Count > 0)
        {
            if (point < patrolPoints.Count)
            {                
                npc.SetDestination(patrolPoints[point].position);
            }
        }
        else
        {
            AssignPlayerPosition();            
            npc.SetDestination(playerPosition);
            OnChasingPlayer();
        }
    }

    void DetectPlayer()
    {
        if (DetectingPlayer(detectionDistance))
        {
            isPlayerDetected = true;
            npcDetectingPlayerName = npcStatus.actorName;

            AssignPlayerPosition();
            ShowNPCDescriptor();            

            OnPlayerDetectionEnter();            
        }
        else
        {            
            if (isPlayerDetected)
            {
                isPlayerDetected = false;
                HideNPCDescriptor();
                OnPlayerDetectionExit();
            }
        }
    }

    void AssignPlayerPosition()
    {        
        if (Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.player).Count<Collider>() > 0)
        {
            playerPosition = Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.player)[0].transform.position;
        }
        else
        {
            playerPosition = transform.position;
        }
    }

    protected void LookAtPlayer()
    {
        Animate();
        npc.gameObject.transform.LookAt(playerPosition);
    }

    protected bool DetectingPlayer(float distance)
    {
        bool isDetected = Physics.CheckSphere(transform.position, distance, Layers.player);
        return isDetected;
    }

    public int ReadPatrolPoint() { return point; }

    public void SetPatrolPoint(int index)
    {        
        GetComponents();

        if (index == c_inicialPoint) { index = patrolPoints.Count; }
        point = index - 1;
        AsignNextDestination();
    }

    public float PlayerDetectionDistance() { return detectionDistance; }

    public void ShowNPCDescriptor()
    {
        if (npcDescriptor)
        {
            npcDescriptor.GetComponentInChildren<TextMeshProUGUI>().text = npcStatus.description;
            npcDescriptor.SetActive(true);
        }
    }

    public void HideNPCDescriptor()
    {
        if (npcDescriptor && npcDetectingPlayerName == npcStatus.actorName)
        {
            npcDescriptor.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
            npcDescriptor.SetActive(false);
            npcDetectingPlayerName = string.Empty;
        }
    }

    virtual protected void GetComponents()
    {
        if (!animator) { animator = GetComponent<Animator>(); }
        if (!npc) { npc = GetComponent<NavMeshAgent>(); }
        if (!npcStatus) { npcStatus = GetComponent<Status>(); }

        // Assign code here when you need to obtain more npc componentes.
    }

    virtual protected void OnInitializeNPC()
    {
        // Assign code here child classes to initalize NPC in Start Unity event.
    }

    virtual protected void OnPlayerDetectionEnter()
    {
        // Assign code here child classes when player is in the npc range detection.
    }

    virtual protected void OnChasingPlayer()
    {
        // Asign code here in child classes whenc chasing player.
    }

    virtual protected void OnPlayerDetectionExit()
    {
        // Assign code here in child classes when player is out of the npc detection range.
    }
}
