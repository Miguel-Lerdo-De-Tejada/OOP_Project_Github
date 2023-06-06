using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField, Tooltip("Explosion delay")] float delay = 3f;
    [SerializeField, Tooltip("Explosion radius")] float radius = 5f;
    [SerializeField, Tooltip("Explosion force")] float force = 700f;
    [SerializeField, Tooltip("Explosion effect")] GameObject explosionEffect;

    Collider[] nearbyObjects;
    Collider playerCollider;
    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        PlayCountdown();
        if (IsCountdownFinish()) { Explode(); }
    }

    void InitializeCountdown()
    {
        countdown = delay;
    }

    void PlayCountdown()
    {
        countdown -= Time.deltaTime;
    }

    bool IsCountdownFinish()
    {
        return countdown <= 0 && !hasExploded;
    }

    void Explode()
    {
        PlayExplosionEffect();
        DetectNerbyObjects();
        ApplyExplosionForceToObjects();
        DestroyBullet();
        DeactivateNextExplosion();
    }

    void PlayExplosionEffect()
    {
        Instantiate(explosionEffect,transform.position,transform.rotation);
    }

    void DetectNerbyObjects()
    {
        nearbyObjects = Physics.OverlapSphere(transform.position, radius);
        playerCollider = Physics.OverlapSphere(transform.position, radius, Layers.player)[0];
    }

    void ApplyExplosionForceToObjects()
    {
        foreach(Collider currentObject in nearbyObjects)
        {
            // Add force.
            AddForce(currentObject);

            // Damage.
        }        
    }

    void AddForce(Collider currentObject)
    {
        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        if (rb != null) { rb.AddExplosionForce(force, transform.position, radius); }        
    }

    void AddForceToPlayer()
    {
        CharacterController controler = playerCollider.GetComponent<CharacterController>();
        controler.attachedRigidbody.AddExplosionForce(force, transform.position, radius);        
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void DeactivateNextExplosion()
    {
        hasExploded = true;
    }
}
