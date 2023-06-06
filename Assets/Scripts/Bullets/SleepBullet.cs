using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepBullet : StandardBullet
{
    /* Poison bullet inherits from SandardBullet subbase class and base BulletProjectile Class:
     Use OnChangeTargetStatus and the status parameter to sleep the target.*/

    protected override void OnChangeTargetStatus(Status status)
    {
        status.sleep = true;
    }
}
