using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [Header("Bullet:")]
    [SerializeField, Tooltip("Bullet prefab")]          private GameObject bullet;
    [SerializeField, Tooltip("Fire start position")]    private Transform aimPosition;
    [SerializeField, Tooltip("Bullet speed")]           private float bulletSpeed;
    [SerializeField, Tooltip("Fire effect")]            private ParticleSystem fireEffect;

    [Header("Time to fire:")]
    [HideInInspector, Tooltip("Is the bullet fired")]   public bool isFired;
    [SerializeField, Tooltip("Start fire")]             private float start;
    [SerializeField, Tooltip("Repeat fire")]            private float repeat;    

    private const string cFireBullets = "Fire";    

    private void Awake()
    {        
        isFired = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        BeguinShooting();
    }

    void BeguinShooting()
    {
        InvokeRepeating(cFireBullets, start, repeat);
    }

    void Fire()
    {        
        if (IsAimPositionAndBulletAssigned())
        {
            if (IsNPCActiveInScene()) 
            {
                ShootBullet();
                PlayShootEffect();
            }
        }
    }

    bool IsAimPositionAndBulletAssigned()
    {
        return aimPosition != null && bullet != null;
    }

    bool IsNPCActiveInScene()
    {
        return gameObject.activeInHierarchy? isFired: false;
    }

    void ShootBullet()
    {
        GameObject bulletPrefab;
        bulletPrefab = Instantiate(bullet, aimPosition.position, gameObject.transform.rotation);
        Rigidbody bulletRb = bulletPrefab.GetComponent<Rigidbody>();
        bulletRb.AddForce(aimPosition.forward * bulletSpeed, ForceMode.Impulse);
    }

    void PlayShootEffect()
    {
        if (fireEffect != null) { fireEffect.Play(); }
    }
}
