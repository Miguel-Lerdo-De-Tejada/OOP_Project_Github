using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName ="New stats data", menuName ="Character stats")]
public class Stats : ScriptableObject
{
    // Description:
    #region    
    [Space(3), Header("Character description:"), Space(3)]
    [Tooltip("character sprite image in the Dialogues.")] public Texture characterSpriteImage;
    [Tooltip("Name")] public string characterName;
    [Tooltip("Description"), TextArea(4, 8)] public string description;
    #endregion

    // Level system:
    #region    
    [Space(6), Header("Level stats:"), Space(3)]
    [Tooltip("Level")] public int level;
    [Tooltip("MaxLevel")] public int maxLevel;
    [Tooltip("Experience")] public int experience;
    [Tooltip("MaxExperience")] public int maxExperience;
    #endregion

    // Vital stats:
    #region
    [Space(6), Header("Vital stats:"), Space(3)]
    [Tooltip("Health")] public int health;
    [Tooltip("MaxHealth")] public int maxHealth;
    [Tooltip("Mana")] public int mana;
    [Tooltip("MaxMana")] public int maxMana;
    #endregion

    // Standard stats:
    #region
    [Space(6), Header("Standard stats:"), Space(3)]
    [Tooltip("Strength")] public int strength;
    [Tooltip("Ednurance")] public int endurance;
    [Tooltip("Dexterity")] public int dexterity;
    [Tooltip("Intelligence")] public int intelligence;
    [Tooltip("Wisdom")] public int wisdom;
    [Tooltip("Magic power")] public int magicPower;
    [Tooltip("Magic resistance")] public int resistance;
    #endregion

    // Vital stats attributes stats:
    #region
    [Space(6), Header("Vital attributs stats:")]
    [Tooltip("Death")] public bool death = false;
    [Tooltip("Good or bad soul for kill a npc or an enemy.")] public bool badSoul = false;
    #endregion

    // Bad attributes stats:
    #region
    [Space(6), Header("Bad attributes stats:"), Space(3)]
    [Tooltip("Poison")] public bool poison;
    [Tooltip("Burn")] public bool burn;
    [Tooltip("Frost")] public bool frost;
    [Tooltip("Blind")] public bool blind;
    [Tooltip("Sleep")] public bool sleep;
    [Tooltip("Silence")] public bool silence = false;
    [Tooltip("Stop")] public bool stop;
    [Tooltip("Slow")] public bool slow;
    [Tooltip("Berseker")] public bool berseker;
    [Tooltip("Petrified")] public bool petrified;
    [Tooltip("Mermaid fell in love")] public int mermaidFellInLove = 0;
    [Tooltip("Max mermaid fell in love.")] public int maxMermaidFellInLove = 100;
    [Tooltip("DeathCount")] public int deathCount;
    #endregion

    // Good attributes stats:
    #region
    [Space(6), Header("Good attributes stats:"), Space(3)]
    [Tooltip("Heath Regen")] public bool regen;
    [Tooltip("Mana regen.")] public bool manaRegen;
    [Tooltip("Protect")] public bool protect;
    [Tooltip("Shell")] public bool shell;
    [Tooltip("Hast")] public bool haste;
    [Tooltip("Inmune")] public bool inmune;
    [Tooltip("Weapon henance")] public int weaponHenance;
    [Tooltip("Armour enhance")] public int armourHenhance;
    #endregion

    // Rewards:
    #region
    [Space(6),Header("Rewards to player after the npc or enemy is killed."),Space(3)]
    [Tooltip("Experience obtained to kill the npc or enemy.")] public int experienceReward;
    #endregion
}