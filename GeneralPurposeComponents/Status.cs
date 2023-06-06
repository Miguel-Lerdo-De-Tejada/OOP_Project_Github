using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status : MonoBehaviour
{
    // attributes.
    [Header("Gamer description")]
    [Tooltip("The name of the gamer who gides the player in this adventure")] public string gamerName;
    [Tooltip("Number of enemies vanquished in this game")] public int enemiesKilled;

    [Header("Player description:")]
    [HideInInspector] public int id;    
    [Tooltip("Image of the actor, NPC or player")] public Texture actorSpriteImage;    
    [Tooltip("Name of actor, player or NPC.")] public string actorName;
    [Tooltip("Description of actor."), TextArea(4, 3)] public string description;

    [Space(7)]

    [Header("Player level:")]
    [Tooltip("Player Max lever:")] public int maxLevel;
    [Tooltip("Level")] public int level;
    [Tooltip("Experience for next lever")] public int experienceNextLevel;
    [Tooltip("Experience rate grouth for next level.")] public int experienceGrouthRate;
    [Tooltip("Current experience.")] public int experience;    
    
    public int Max_health { get { return max_health; } private set { value = max_health; }}

    [Space(7)]

    [Header("Player básic attributes")]
    [Tooltip("Resurrections")] public int resurrections;
    [SerializeField, Tooltip("Max health.")] public int max_health;
    [Tooltip("Health")] public int health;

    public int Max_mana { get { return max_mana; } private set { value = max_mana; } }
    [SerializeField, Tooltip("Max mana.")] public int max_mana;
    [Tooltip("Mana")] public int mana;

    [Space(7)]

    [Header("Player general attributes")]
    [SerializeField, Tooltip("Strength")] public int strength;
    [SerializeField, Tooltip("Endurance")] public int endurance;
    [SerializeField, Tooltip("Dexterity")] public int dexterity;
    [SerializeField, Tooltip("Wisdom")] public int wisdom;
    [SerializeField, Tooltip("Faith")] public int faith;
    [SerializeField, Tooltip("Resistance")] public int resistance;

    // States.

    // Bad:
    [Header("Player status attributes")]
    [Tooltip("Death")] public bool death;
    [HideInInspector] public bool sleep;
    [HideInInspector] public bool poison;
    [HideInInspector] public bool hungry;
    [HideInInspector] public bool blood;
    [HideInInspector] public bool burn;
    [HideInInspector] public bool slow;
    [HideInInspector] public bool stop;
    [HideInInspector] public bool confuse;
    [HideInInspector] public bool berserk;
    [HideInInspector] public bool posess;

    // Good:
    [HideInInspector] public bool protect;     // Enhance endurance.
    [HideInInspector] public bool resist;      // Enhance resistance.
    [HideInInspector] public bool reflect;     // Reflect a spell.
    [HideInInspector] public bool bost;        // Bost strength.
    [HideInInspector] public bool magicBost;   // Bost wisdom.
    [HideInInspector] public bool quick;       // Bost player speed;
        
    // If NPC or Player is killed offer experience and check if the NPC wich is killed is good or bad. In case that he or she is good, you will have
    // bad Alignment and your soul will be send to Enusbel, and the game is over.
    [Header("If NPC or Player is killed:")]
    [Tooltip("Experience reward.")] public int experienceReward;
    [Tooltip("Alignment if player or NPC killed a good or bad NPC")] public bool goodAlignment;

    // If NPC is activated by a teleporter.
    [Header("Is NPC activated by a teleperter:")]
    [Tooltip("Is NPC teleported")] public bool isTeleported;
    [Tooltip("Is NPC rightnow activated by a teleporter")] public bool isTeleportingActivated;


     
    private void Awake()
    {        
        InitializeBasicAttributes();
    }

    void InitializeBasicAttributes()
    {        
        health = max_health;
        mana = max_mana;
    }
}
