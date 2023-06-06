using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBullet : StandardBullet
{
    /* Poison bullet inherits from SandardBullet subbase class and base BulletProjectile Class:
     Use OnChangeTargetStatus and the status parameter to kill the target with the death status attribute.*/

    protected override void OnChangeTargetStatus(Status status)
    {
        status.death = true;
    }
}
