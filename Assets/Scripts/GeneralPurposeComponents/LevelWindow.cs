using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Level text")] TextMeshProUGUI levelText;
    [SerializeField, Tooltip("Experience bar")] Slider experienceSlider;
    [SerializeField, Tooltip("Experience text")] TextMeshProUGUI xpText;
    [SerializeField, Tooltip("Resurrections text")] TextMeshProUGUI resurrectionsText;

    void SetExperienceBarSize(float experience)
    {
        experienceSlider.value = experience;
    }

    void SetExperienceBarMaxSize(float maxExperience)
    {
        experienceSlider.maxValue = maxExperience;
    }

    void SetLevel(int levelNumber)
    {
        levelText.text = levelNumber.ToString();
    }
    void SetExperienceText(int currentXP, int maxXP) { xpText.text = $"{currentXP} / {maxXP}"; }

    void SetResurrectionsText(int resurrections) { resurrectionsText.text = $"Resurrections: {resurrections}"; }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        SetLevel(levelSystem.GetLevelNumber());
        SetExperienceBarMaxSize(levelSystem.GetExperienceNextLevel());
        SetExperienceBarSize(levelSystem.GetExperience());        
        SetExperienceText(levelSystem.GetExperience(), levelSystem.GetExperienceNextLevel());
        SetResurrectionsText(levelSystem.GetResurrections());
    }

    public void SetLevelByStatus(Status status)
    {        
        SetLevel(status.level);
        SetExperienceBarMaxSize(status.experienceNextLevel);
        SetExperienceBarSize(status.experience);        
        SetExperienceText(status.experience, status.experienceNextLevel);
        SetResurrectionsText(status.resurrections);
    }
}
