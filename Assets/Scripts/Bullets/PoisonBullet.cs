using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBullet : StandardBullet
{
    /* Poison bullet inherits from SandardBullet subbase class and base BulletProjectile Class:
     Use OnChangeTargetStatus and the status parameter to poison the target.*/

    protected override void OnChangeTargetStatus(Status status)
    {
        status.poison = true;
    }
}
