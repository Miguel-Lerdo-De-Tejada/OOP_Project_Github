using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class PatrolExp : MonoBehaviour
{
    [Space(3), Header("Patrol points data:"), Space(3)]
    [SerializeField, Tooltip("Drag the objects to patrol")]
    private List<GameObject> patrolPoints;

    [SerializeField, Tooltip("Patrol points detect radius"), Range(1,3)]
    private float detectRadius;

    [SerializeField, Tooltip("NPC UI descriptor")] 
    private GameObject npcDescriptor;
    
    protected Animator animator;
    protected NavMeshAgent agent;    
    protected bool isPatrolling;
    protected bool isStopped = false;
    protected bool isWaiting = false;

    private Status status;
    private int point = 0;
    private float waitUntilNextPoint = 0;
    private float agentSpeed = 0;
    private string currentNPC = string.Empty;

    void Awake()
    {
        GetComponents();        
        ExtendAwake();

        void GetComponents() { status = GetComponent<Status>(); }
    }

    private void Start()
    {
        InitializePatrol();
        ExtendStart();
    }

    private void Update()
    {        
        CheckNextDestination();
        ExtendUpdate();        
    }

    // Awake:
    void InitializePatrol()
    {
        agent = GetComponent<NavMeshAgent>();
        point = 0;
        
        if (IsPointsToPaltorl()) 
        {
            isPatrolling = true;
            LookAtPatrolPoint();
            SetNPCNextDestination();            
        } 
        else 
        { 
            isPatrolling = false; 
        }
        
        void SetNPCNextDestination() { agent.SetDestination(patrolPoints[point].gameObject.transform.position); }        
    }

    // Update:
    void CheckNextDestination()
    {
        if (IsPointsToPaltorl()) 
        {            
            SetNextPatrolPoint(); 
        } 
        else 
        {
            if (IsFriend())
            {                
                StopNPC();
            }
            else
            {
                if (IsPlayer())
                {
                    ChasePlayer(); // Persigue al jugador.
                }

                bool IsPlayer() { return Sensor.Detect(Layers.player, transform.position, Mathf.Infinity); }
                void ChasePlayer() 
                {
                    GameObject player;
                    player = GetPlayer();
                    agent.SetDestination(PlayerPosition());

                    GameObject GetPlayer() { return Sensor.GetNearbyGameObjects(Layers.player, transform.position, Mathf.Infinity)[0]; }
                    Vector3 PlayerPosition() { return player.transform.position; }
                }
            }
        }

        
        void SetNextPatrolPoint()
        {            
            List<Collider> detectedPoints = new List<Collider>();
            DetectNearPatrolPoints();

            foreach (Collider patrolPoint in detectedPoints)
            {
                if (IsPatrolPointDetected())
                {
                    WaitPoint waitPoint;
                    
                    if (IsWaitPoint())
                    {                        
                        StopNPC();
                        SetWaitTime(true);
                        LookAtWaitPointGameObject();
                        
                        if (IsWaitTimeElapsed())
                        {                            
                            SetNextDestination();
                            InitializeWaitPoint();
                            MoveNPC();
                            SetWaitTime(false);
                            LookAtPatrolPoint();

                            void InitializeWaitPoint() { waitUntilNextPoint = 0; }                            
                        }
                        void SetWaitTime(bool isActive) { isWaiting = isActive; }
                        void LookAtWaitPointGameObject() 
                        { 
                            if (waitPoint.ReadLookAtGameObject()) { gameObject.transform.LookAt(waitPoint.ReadLookAtGameObject().transform.position); } 
                        }
                    }
                    else
                    {
                        LookAtPatrolPoint();
                        SetNextDestination();
                    }

                    bool IsWaitPoint() 
                    {
                        waitPoint = patrolPoints[point].gameObject.GetComponent<WaitPoint>();                        
                        return waitPoint;
                    }
                    bool IsWaitTimeElapsed()
                    {                        
                        waitUntilNextPoint += Time.deltaTime;                        
                        return waitUntilNextPoint >= waitPoint.ReadWaitTime();
                    }
                    void LookAtPatrolPoint() { gameObject.transform.LookAt(patrolPoints[point].transform.position); }
                    void SetNextDestination()
                    {
                        point++;
                        if (point >= patrolPoints.Count)
                        {
                            point = 0;
                            CheckNewPatrol();
                        }

                        agent.SetDestination(patrolPoints[point].gameObject.transform.position);

                    }
                }
                bool IsPatrolPointDetected() { return patrolPoint.gameObject.name == patrolPoints[point].gameObject.name; }
            }
            void DetectNearPatrolPoints() { detectedPoints.AddRange(Physics.OverlapSphere(transform.position, detectRadius, Layers.patrolPoint)); }
        }
        bool IsFriend() { return status.goodAlignment; }
       
    }

    bool IsPointsToPaltorl() { return patrolPoints.Count > 0; }

    protected void SetNPCSpeed(float currentSpeed) 
    { 
        if (agentSpeed != currentSpeed) 
        { 
            agentSpeed = currentSpeed;
            agent.speed = agentSpeed;
        } 
    }

    protected void StopNPC()
    {
        agent.isStopped = true;

        if (agent.speed != 0)
        {
            agent.speed = 0;
            isStopped = true;
        }
    }

    protected void MoveNPC()
    {
        if (agent.isStopped)
        {
            agent.speed = agentSpeed;
            agent.isStopped = false;
            isStopped = false;
        }
    }

    protected void LookAtPatrolPoint() { transform.LookAt(patrolPoints[point].transform.position); }

    protected void ShowNPCDescriptor()
    {
        if (npcDescriptor)
        {
            currentNPC = status.actorName;
            npcDescriptor.GetComponentInChildren<TextMeshProUGUI>().text = status.description;
            npcDescriptor.SetActive(true);
        }
    }

    protected void HideNPCDescriptor()
    {
        if (npcDescriptor && currentNPC == status.actorName)
        {
            currentNPC = string.Empty;
            npcDescriptor.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
            npcDescriptor.SetActive(false);
        }
    }



    virtual protected void ExtendAwake()
    {
        // It extends the code of this component to derive diferent kind of patrols in the awake event.
    }

    virtual protected void ExtendStart()
    {
        // It extends the code of this component to derive diferent kind of patrols ins the start event.
    }

    virtual protected void ExtendUpdate()
    {
        // It extends the code of this component to derive diferent kind of patrols in the update event.
    }

    virtual protected void CheckNewPatrol()
    {
        // It extends the code to do something more when the NPC begins a round of new patrol points.        
    }
}
        