using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularDoorManager : MonoBehaviour
{
    [SerializeField, Tooltip("Player detection radius"), Range(0.1f, 3)] float playerDetectionRadius = 3.0f;
    Animator doorAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponents();

        void GetComponents()
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        PerformDoorAction();
    }

    void PerformDoorAction()
    {
        GameObject player;

        if (PlayerDetected())
        {
            player = GetPlayerGameObject();
            OpenDoor(player, true);
        }
        else
        {
            player = gameObject;
            OpenDoor(player, false);
        }

        bool PlayerDetected() { return Sensor.Detect(Layers.player, transform.position, playerDetectionRadius); }
        GameObject GetPlayerGameObject() { return Sensor.GetNearbyGameObjects(Layers.player, transform.position, playerDetectionRadius)[0]; }
    }

    private void OpenDoor(GameObject player, bool isOpen) {if (doorAnimator){ doorAnimator.SetBool(AnimParam.nearbyDoor, isOpen); }}
}
