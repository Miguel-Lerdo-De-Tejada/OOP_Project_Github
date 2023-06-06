using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineManager : MonoBehaviour
{
    PlayableDirector director;    

    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents();
    }

    void InitializeComponents()
    {
        director = GameObject.FindObjectOfType<PlayableDirector>();        
    }

    public double ReadTime() { return director.time; }

    public void SetTime(double loadedTime) 
    { 
        if (!director) { InitializeComponents(); }
        director.time = loadedTime;
    }

    public TimelineAsset ReadCurrentTimeline() { return director.GetComponentInChildren<TimelineAsset>(); }

    public void SetCurrentTimeLine(TimelineAsset loadedTimeLine) { director.Play(loadedTimeLine); }
}
