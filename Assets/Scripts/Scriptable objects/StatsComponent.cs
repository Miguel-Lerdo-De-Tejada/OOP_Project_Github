using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    [SerializeField,Tooltip("Character stats scriptable object")]
    Stats stats;

    public Stats Read () { return stats; }

    public void Write(Stats outerStats) 
    {
        
        stats = outerStats;
    }
}
