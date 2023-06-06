using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruction : StandardHarm
{
    protected override void OnSelfDestruction()
    {
        GetComponent<Status>().health = 0;
        GetComponent<Status>().death = true;
        GetComponent<Patrol>().HideNPCDescriptor();
        gameObject.SetActive(false);
    }
}
