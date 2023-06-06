using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarManager : MonoBehaviour
{
    /* This class sets the values of the UIBar, to initialize the UIBar use the SetMaxHealth method, and to actualize the UIBar use the
    SetBarValue in other classes when instanciate this class.
    
     Use the UIBarManager component in any kind of player or NPCs attributes wich require a bar, like health, mana, energy, and so on.It represents a 
    bar of how many health, mana, or any was spent in the game, or if you acomulates the necesary energy to cast a spell or use a technic.*/

    [Tooltip("Slider to manipulate the healthbar.")] public Slider slider;
    [SerializeField, Tooltip("Color gradient of the healthbar.")] Gradient gradient;
    [SerializeField, Tooltip("Drag here the fill image of the healthbar.")] Image fillImage;

    public void SetCurrentValue(int value)
    {
        slider.value = value;
        fillImage.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        fillImage.color = gradient.Evaluate(1f);
    }
}
