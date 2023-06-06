using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardBullet : BulletProjectile
{
    /* Sub base class StandardBullet which inherit form BulletProjectile:
     * This class is intended to give a bullet basic damage operation, If you want to inflict status change in a target use the ChangeTargetStatus method in
     child classes, you do not need to use Monobehaviour methods becuase this methods are manipulated in the BulletProjectile base class.
     If you want to create a bullet which only change the target status, set the Strength parameter to 0. If you want bullets to health a targe, set
     negative numbers in the strength parameter in the editor.*/

    [SerializeField,Tooltip("Bullet attack strength")] int strength = 4;
    [SerializeField, Tooltip("Target layer")] LayerMask targetMask;

    // HealthBarManager oponentHealthBar;
    Status status;
    HealthBarManager healthBar;

    private const int c_minHealth = 0;

    protected override void OnDamageTarget(Collision target)
    {        
        DamageTarget(target);
        
    }

    void DamageTarget(Collision target)
    {
        if (MaskContainLayer(targetMask, target.gameObject.layer))
        {
            ObtainStatusComponent(target);
            ObtainHealthBar(target);

            if (HaveStatusComponent())
            {
                ApplyDamage();
                if (IsDeath()) { SetDeathStatus(); }
                ApplyHealthToHealthBar();
                npcIsFriend();
                OnChangeTargetStatus(status);

                void npcIsFriend()
                {
                    if (Layers.ReadLayer(target.gameObject.layer) != Layers.player)
                    {
                        if (status.goodAlignment)
                        {
                            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
                            gameManager.SetPlayerRunned();                            
                        }                        
                    }                    
                }
            }
        }
    }

    void ObtainStatusComponent(Collision target) 
    {
        status = target.gameObject.GetComponent<Status>();
    }

    void ObtainHealthBar(Collision target)
    {
        healthBar = target.gameObject.GetComponent<HealthBarManager>();
    }

    bool HaveStatusComponent()
    {
        return status != null;
    }

    bool IsDeath()
    {        
        return status.health <= c_minHealth;
    }

    private void ApplyDamage()
    {
        status.health -= strength;
        status.health = Mathf.Clamp(status.health, c_minHealth, status.Max_health);
    }

    private void SetDeathStatus()
    {        
        status.death = true;
    }

    private void ApplyHealthToHealthBar()
    {
        healthBar.SetHealth(status.health);
    }

    bool MaskContainLayer(LayerMask mask, int layer)
    {
        bool layerValue = ((1 << layer) | mask.value) == mask.value;
        return layerValue;
    }

    public int ReadStrength() { return strength; }
    protected virtual void OnChangeTargetStatus(Status status)
    {
        // Change target status in child classes.
    }
}
