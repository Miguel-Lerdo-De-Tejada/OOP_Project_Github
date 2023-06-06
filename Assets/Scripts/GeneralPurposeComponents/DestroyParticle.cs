using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField, Tooltip("Seconds to wait to destroy particle"), Range(0.1f, 3.0f)] float waitTime = 0.1f;

    void Awake()
    {
        DestroyParticleObject();

        void DestroyParticleObject()
        {
            StartCoroutine(destroyGameObject());
        }
    }

    IEnumerator destroyGameObject()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
