using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereRobotPatrol : MonoBehaviour
{
	Vector3 rot = Vector3.zero;
	
	int point = 0;
	bool isRool = false;

	Animator anim;
	NavMeshAgent robot;

	[SerializeField, Tooltip("Drag here the sphere robot patrol points.")]
	List<GameObject> patrolPoints = new List<GameObject>();

	struct AnimsParam
	{
		public static string walk = "Walk_Anim";
		public static string roll = "Roll_Anim";
		// public static string open = "Open_Anim";
	}

	// Use this for initialization
	void Awake()
	{
		anim = gameObject.GetComponent<Animator>();
		robot = GetComponent<NavMeshAgent>();

		gameObject.transform.eulerAngles = rot;
	}

	private void Start()
	{
		anim.SetBool(AnimsParam.walk, true);		
		robot.destination = patrolPoints[point].gameObject.transform.position;

		isRool = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == patrolPoints[point].gameObject.name)
		{
			point++;
			if (point >= patrolPoints.Count) 
			{ 
				point = 0;
				isRool = isRool ? false : true;

				if (isRool)
				{					
					anim.SetBool(AnimsParam.walk, false);					
					anim.SetBool(AnimsParam.roll, true);
				}
				else
				{					
					anim.SetBool(AnimsParam.walk, true);					
					anim.SetBool(AnimsParam.roll, false);
				}
			}
			
			robot.destination = patrolPoints[point].gameObject.transform.position;
		}
	}
}