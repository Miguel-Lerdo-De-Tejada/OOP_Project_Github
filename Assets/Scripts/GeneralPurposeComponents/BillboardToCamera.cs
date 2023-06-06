using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardToCamera : MonoBehaviour
{
    // BillboardToCamera is intended to position a UI object in world space is looking at the camera always.

    [SerializeField,Tooltip("Drag your camera here.")]Transform cam;

    // Update is called once per frame
    void LateUpdate()
    {
        LookAtCamera();
    }

    void LookAtCamera()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
