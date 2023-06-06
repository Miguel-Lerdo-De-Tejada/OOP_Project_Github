using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeath : Pickup
{
    [SerializeField,Tooltip("Amount of health wich recieve the player")]int amount;

    protected override void OnAction()
    {        
        HealthPlayer();
    }

    void HealthPlayer()
    {        
        playerStatus.health += amount;        
        playerStatus.health = Mathf.Clamp(playerStatus.health, 0,playerStatus.Max_health);
        playerHealthBar.SetHealth(playerStatus.health);
    }
}
