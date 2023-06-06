using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatusManager : MonoBehaviour
{
    [SerializeField, Tooltip("stats player foto sprite: ")]         RawImage statusPlayerPhoto;
    [SerializeField, Tooltip("List of UI stats text fields: ")]     List<TextMeshProUGUI> statusTextFields = new List<TextMeshProUGUI>();

    enum UIStatusTextField
    {
        gamer,
        enemiesKilled,
        name,
        description,
        leve,
        experience,
        nextLevel,
        health,
        maxHealth,
        bulletName,
        bulletStrength,        
    }

    public void PrintInUIStatus(GameObject player, Status status)
    {
        StandardBullet bullet = GetBullet();
        printPlayerStatus();

        StandardBullet GetBullet()
        {
            ThirdPersonShooterControllerExp controller = player.GetComponent<ThirdPersonShooterControllerExp>();
            if (controller)
            {
                StandardBullet standardBulletComponent = controller.ReadBullet().GetComponent<StandardBullet>();
                return standardBulletComponent;
            }
            else
            {
                return null;
            }

        }
        void printPlayerStatus()
        {
            statusPlayerPhoto.texture = status.actorSpriteImage;
            statusTextFields[(int)UIStatusTextField.gamer].text = $"Gamer: {status.gamerName}";
            statusTextFields[(int)UIStatusTextField.enemiesKilled].text = $"Enemies vanquished: {status.enemiesKilled}";
            statusTextFields[(int)UIStatusTextField.name].text = status.actorName;
            statusTextFields[(int)UIStatusTextField.description].text = status.description;
            statusTextFields[(int)UIStatusTextField.leve].text = $"Leve: {status.level}";
            statusTextFields[(int)UIStatusTextField.experience].text = $"Experience: {status.experience}";
            statusTextFields[(int)UIStatusTextField.nextLevel].text = $"Next level: {status.experienceNextLevel}";
            statusTextFields[(int)UIStatusTextField.health].text = $"Health: {status.health}";
            statusTextFields[(int)UIStatusTextField.maxHealth].text = $"Max health: {status.Max_health}";
            if (bullet)
            {
                statusTextFields[(int)UIStatusTextField.bulletName].text = $"Bullet name: {bullet.name}";
                statusTextFields[(int)UIStatusTextField.bulletStrength].text = $"Bullet strength: {bullet.ReadStrength()}";
            }
            else
            {
                statusTextFields[(int)UIStatusTextField.bulletName].text = "Fares soul don't use bullets";
                statusTextFields[(int)UIStatusTextField.bulletStrength].text = "No bullet strength";
            }
        }
    }
}
