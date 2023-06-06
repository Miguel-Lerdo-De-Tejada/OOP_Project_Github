using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    /* This class sets the values of the health bar, to actualize the healthbar use the SetHealth in other classes when instanciate this class, 
     * for example, when player or NPC recieve the impact of a bullet shooted.*/

    [Tooltip("Drag an UIBar to represent the health bar.")] public UIBarManager healthBar;
    [HideInInspector] public Status status;

    private void Start()
    {
        GetComponents();        
        InitializeHealthBar();

        void GetComponents()
        {
            status = GetComponent<Status>();
        }
        void InitializeHealthBar()
        {
            ActualizeMaxHealth(status.max_health, status.health);
        }
    }



    public void SetHealth(int health)
    {
        healthBar.SetCurrentValue(health);
    }

    public void ActualizeMaxHealth(int maxHealth, int health)
    {
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetCurrentValue(health);
    }
}
