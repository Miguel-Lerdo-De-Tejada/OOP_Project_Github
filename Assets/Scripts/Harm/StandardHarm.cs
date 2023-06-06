using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardHarm : Harm
{
    /* Sub base class StandardHarm which inherit form Harm:
     This class is intended to give a harm game object basic damage operation, If you want to inflict status change in a target use the 
     ChangeTargetStatus method in child classes, you do not need to use Monobehaviour methods becuase this methods are manipulated in the Harm base class.
     If you want to create a harm game object which only change the target status, set the Strength parameter to 0. If you want the harm game object to 
     health a targe, set negative numbers in the strength parameter in the editor.*/
    
    Status status;
    HealthBarManager healthBar;

    private const int c_minHealth = 0;

    protected override void OnDamageTarget(GameObject target)
    {
        DamageTarget(target);        
    }

    protected override void OnDamageTarget(Collider target)
    {
        DamageTarget(target);        
    }

    void DamageTarget(GameObject target)
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

                OnChangeTargetStatus(status);
            }
        }
    }

   void DamageTarget(Collider target)
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

                OnChangeTargetStatus(status);                
            }

            OnSelfDestruction();
        }
    }

    void ObtainStatusComponent(GameObject target)
    {
        status = target.GetComponent<Status>();
    }

    void ObtainStatusComponent(Collider target)
    {
        status = target.GetComponent<Status>();
    }

    void ObtainHealthBar(GameObject target)
    {
        healthBar = target.GetComponent<HealthBarManager>();
    }

    void ObtainHealthBar(Collider target)
    {
        healthBar = target.GetComponent<HealthBarManager>();
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

    protected virtual void OnSelfDestruction() 
    { 
        // Destruct game object after harming target.
    }

    protected virtual void OnChangeTargetStatus(Status status)
    {
        // Change target status in child classes.
    }
}
