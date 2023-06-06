using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField, Tooltip("Angle"), Range(1, 360)] int rotationAngle;
    [SerializeField, Tooltip("Ratio"), Range(1, 45)] int rotateRatio;

    int currentRotation;
    int yRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        if (currentRotation <= 0) { currentRotation = rotationAngle; } else if (currentRotation >= rotationAngle) { currentRotation = 0; }
        int yRotation = Mathf.RoundToInt(Mathf.Lerp((float)transform.rotation.y, (float)rotationAngle, (float)rotateRatio));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
    }
}
