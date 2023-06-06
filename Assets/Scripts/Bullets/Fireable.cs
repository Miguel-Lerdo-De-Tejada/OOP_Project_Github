using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireable : MonoBehaviour
{
    [SerializeField, Tooltip("Damage stregth")] int damageStrength;
    [SerializeField, Tooltip("Particle system explotion")] ParticleSystem explotion;

    struct tags
    {
        public const string obstacle = "Obstacle";
        public const string player = "Player";
    }

    private void OnCollisionEnter(Collision collision)    
    {
        if (collision.gameObject.CompareTag(tags.player) || collision.gameObject.CompareTag(tags.obstacle)) 
        { 
            Destroy(gameObject);
            explotion.Play();
        }
        if (collision.gameObject.CompareTag(tags.player)) { DamagePlayer(); }
    }

    private void DamagePlayer()
    {
        // Damage player:
        Debug.Log("Shoot");
    }
}
