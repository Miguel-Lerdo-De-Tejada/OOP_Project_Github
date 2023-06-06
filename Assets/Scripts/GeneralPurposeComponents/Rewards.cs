using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewards : MonoBehaviour
{
    [Header("Level & experience grouth parameters.")]
    [SerializeField, Tooltip("Experience grouth step"), Range(1, 200)]      int   experienceGrouth = 1;
    [SerializeField, Tooltip("Set here the Level UI")]                      LevelWindow levelWindowUI;

    [Header("Stats rewards")]
    [SerializeField, Tooltip("Health grouth range reward"), Range(1, 10)]  int healthGrouthRate = 2;

    private Status      status;
    private GameObject  player;
    private Status      playerStatus;
    private bool        isAdded = false;
    private int         xpToAdd = 0;
    private const int   c_minGrouth = 1;
        
    private void Awake()
    {
        GetComponents();

        void GetComponents()
        {
            status = GetComponent<Status>();
        }
    }

    private void Update()
    {
        AddRewardExperienceToPlayer();

        void AddRewardExperienceToPlayer()
        {
            if (IsXPToAdd())
            {                
                AddGrouthExperience();
                CheckPlayerNextLevel();
                ExperienceBoundaries();
                SetLevelWindowUI();
                decreaceXPToAdd();


                void AddGrouthExperience()
                {
                    playerStatus.experience += experienceGrouth;                    
                }
                void CheckPlayerNextLevel()
                {
                    if (IsPlayerNextLevel())
                    {
                        AddLevel();
                        AddHealthReward();

                        void AddLevel()
                        {
                            playerStatus.level++;
                            playerStatus.experience = 0;
                            playerStatus.experienceNextLevel *= playerStatus.experienceGrouthRate;
                        }
                        void AddHealthReward()
                        {
                            HealthBarManager healthBar;


                            int grouthRate = Random.Range(c_minGrouth, healthGrouthRate + 1);

                            playerStatus.max_health *= grouthRate;
                            healthBar = player.GetComponent<HealthBarManager>();
                            if (healthBar) { healthBar.ActualizeMaxHealth(playerStatus.Max_health, playerStatus.health); }
                        }

                    }

                    bool IsPlayerNextLevel() { return playerStatus.experience > playerStatus.experienceNextLevel; }
                }
                void ExperienceBoundaries()
                {
                    playerStatus.experience = Mathf.Clamp(playerStatus.experience, 0, playerStatus.experienceNextLevel);
                }
                void SetLevelWindowUI()
                {
                    levelWindowUI.SetLevelByStatus(playerStatus);
                }
            }

            bool IsXPToAdd() { return xpToAdd > 0; }
            void decreaceXPToAdd() { xpToAdd -= experienceGrouth; }
        }
    }

    public void CheckNpcDeath()
    {        
        if (IsNPCDeath())
        {
            if (!IsRewardAdded())
            {                
                DetectPlayer();
                AddToPlayerEnemiesKilled();
                CalculateXPToAddInUpdateEvent(status.experienceReward);
                                
                void AddToPlayerEnemiesKilled() { playerStatus.enemiesKilled++; }                
            }            
        }

        bool IsNPCDeath() { return status.death; }
    }

    public void CheckMissionAccomplished(bool isMissionAcomplished, int xpReward) 
    { 
        if (isMissionAcomplished)
        {
            if (!IsRewardAdded())
            {
                DetectPlayer();
                CalculateXPToAddInUpdateEvent(xpReward);
            }
        }
    }

    public void InitializeAddReward() { isAdded = isAdded ? false : isAdded; }

    public bool ReadRewardApplyed() { return isAdded; }

    public void SetRewardApplyed(bool isRewardApplyedInDB) { isAdded = isRewardApplyedInDB; }

    bool IsRewardAdded()
    {
        bool rewardAdded = isAdded ? true : false;
        isAdded = true;

        return rewardAdded;
    }

    void DetectPlayer()
    {
        player = Physics.OverlapSphere(transform.position, Mathf.Infinity, Layers.player)[0].gameObject;
        playerStatus = player.GetComponent<Status>();
    }

    void CalculateXPToAddInUpdateEvent(int xpObtained) { xpToAdd = xpObtained; }
}
