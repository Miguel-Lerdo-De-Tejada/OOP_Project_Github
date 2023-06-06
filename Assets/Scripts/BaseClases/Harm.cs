using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harm : MonoBehaviour
{
    /* Harm is a base class: 
     
     Use the methods OnDestroy Bullet and OnSetBulletVelocity if your game object is a projectile to set the bullet velocity and destroy it when collides
    with a target.
    Use OnDamageTarget in child Classes to give a behaviour to a bullet, like damage o or health a targuet, or change a state in a targuet like sleep, 
    death, poison, burn, and so on, and to change the target health bar value.*/

    [Header("Harm object collision parameters")]
    [SerializeField, Tooltip("Harm target effect")] protected Transform vfxHitGreen;
    [SerializeField, Tooltip("Harm any thing effect")] protected Transform vfxHitRed;
    // [SerializeField, Tooltip("Collision sensor radius."), Range(1,3)] float collisionRadius = 1.0f;
    [SerializeField, Tooltip("Target layer")] protected LayerMask targetMask;
    [SerializeField, Tooltip("Harm attack strength")] protected int strength = 4;

    protected Rigidbody rb;

    private void Awake()
    {
        GetHarmComponents();

        void GetHarmComponents() { rb = GetComponent<Rigidbody>(); }
    }

    void Start()
    {
        OnSetBulletVelocity();
    }

    private void Update()
    {
        OnBulletInactive();
    }    

    private void OnTriggerEnter(Collider other)
    {        
        HitTarget(other);        

        void HitTarget(Collider target)
        {
            
            bool isTarget = IsGameObjectATarget();            
            if (isTarget)
            {
                // Hit target                
                if (IsHitParticles()) { PlayHitParticles(); }
                OnDamageTarget(target);
                OnDestroyBullet();
                isTarget = false;
            }

            bool IsGameObjectATarget() { return target.gameObject.GetComponent<BulletTarget>() != null; }
            bool IsHitParticles() { return vfxHitGreen; }
            void PlayHitParticles() { Instantiate(vfxHitGreen, transform.position, Quaternion.identity); }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {        
        HitTarget(collision);
        
        OnDestroyBullet();

        void HitTarget(Collision target)
        {
            bool isTarget = IsGameObjectATarget();
            
            if (isTarget)
            {                
                // Hit target
                if (IsHitParticles()) { OnInstantiateHitParticles(target); }
                OnDamageTarget(target);
            }
            else
            {

                // Hit any
                if (IsFailParticles()) { OnInstantiateFailParticles(target); }
            }

            // DeactiveParticles();

            bool IsGameObjectATarget() { return target.gameObject.GetComponent<BulletTarget>() != null; }
            bool IsHitParticles() { return vfxHitGreen; }            
            bool IsFailParticles() { return vfxHitRed; }            
            // void DeactiveParticles() { gameObject.SetActive(false); }
        }
    }

    protected virtual void OnInstantiateHitParticles(Collision target) 
    { 
        // This method is redefined in inherited objects if the instantiate object is diferent that the harm object, like when a player
        // hits a limit ground, The particles will be instantiated in players position.

        Instantiate(vfxHitGreen, transform.position, Quaternion.identity); 
    }

    protected virtual void OnInstantiateFailParticles(Collision target) 
    {
        // This method is redefined in inherited objects if the instantiate object is diferent that the harm object, like when a player
        // hits a limit ground, The particles will be instantiated in players position.

        Instantiate(vfxHitRed, transform.position, Quaternion.identity); 
    }

    protected virtual void OnDestroyBullet()
    {
        // If the harm game object is a projectile, destroy it.
    }

    protected virtual void OnSetBulletVelocity()
    {
        // If the harm object is a projectile, set the projectile velocity here.
    }

    protected virtual void OnDamageTarget(Collision target)
    {
        // Substract the bullet power attack from the target health.
    }

    protected virtual void OnDamageTarget(Collider target)
    {
        // Substract the bullet power attack from the target health.
    }

    protected virtual void OnDamageTarget(GameObject target)
    {
        // Substract the bullet power attack from the target health.
    }

    protected virtual void OnBulletInactive()
    {
        // If a bullet is inactive.
    }
}
