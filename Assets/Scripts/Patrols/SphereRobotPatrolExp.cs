using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereRobotPatrolExp : PatrolExp
{
	Animator anim;
	
	bool isRool = false;

	// Use this for initialization
	protected override void ExtendAwake()
	{
		InitializeRollBotAnimation();
	}

    protected override void CheckNewPatrol()
    {
		SetRollAnimation();
    }

	void InitializeRollBotAnimation()
    {
		anim = gameObject.GetComponent<Animator>();
		anim.SetBool(AnimParam.walk, true);

		transform.eulerAngles = Vector3.zero;		
		
		isRool = false;
	}

	void SetRollAnimation()
    {
		isRool = isRool ? false : true;

		if (isRool)
		{
			anim.SetBool(AnimParam.walk, false);
			anim.SetBool(AnimParam.roll, true);
		}
		else
		{
			anim.SetBool(AnimParam.walk, true);
			anim.SetBool(AnimParam.roll, false);
		}
	}
}