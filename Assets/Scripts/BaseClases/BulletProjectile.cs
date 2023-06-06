using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    /* BulletProjectile is a base class: 
     
     Use the method OnDamageTarget in child Classes to give a behaviour a bullet, like damage o or health a targuet, or change a state in a targuet, sleep, 
    death, poison, burn, and so on.*/

    [Header("Projectile Shoot:")]
    [SerializeField, Tooltip("Bullet speed")] float speed = 10.0f;
    
    [Space(6)]

    [Header("Projectile Hit")]
    [SerializeField, Tooltip("Shoot Target")] Transform vfxHitGreen;
    [SerializeField, Tooltip("Shoot any thing")] Transform vfxHitRed;

    Rigidbody bulletRigidbody;

    private void Awake()
    {
        GetBulletComponents();
    }

    void Start()
    {
        SetBulletVelocity(speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HitTarget(collision);
        DestroyBullet();        
    }

    void GetBulletComponents()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
    }

    void SetBulletVelocity(float newSpeed)
    {
        bulletRigidbody.velocity = transform.forward * newSpeed;
    }

    void HitTarget(Collision target)
    {
        bool isTarget = IsGameObjectATarget();
                
        if (isTarget)
        {
            // Hit target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);            
            OnDamageTarget(target);
        }
        else
        {            

            // Hit any
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
        }

        bool IsGameObjectATarget() { return target.gameObject.GetComponent<BulletTarget>() != null; }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDamageTarget(Collision target)
    {
        // Substract the bullet power attack from the target health.
    }
}
