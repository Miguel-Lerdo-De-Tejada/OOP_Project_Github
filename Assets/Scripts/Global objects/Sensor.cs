using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sensor
{
    public static bool Detect(int layer, Vector3 origin, float radius) { return Physics.CheckSphere(origin, radius, layer); }

    public static List<Collider> GetNearbyColliders(int layer, Vector3 origin, float radius) 
    { 
        List <Collider> goColliders = new List<Collider>();
        goColliders.AddRange(Physics.OverlapSphere(origin, radius, layer));
        
        return goColliders; 
    }

    public static List<GameObject> GetNearbyGameObjects(int layer, Vector3 origin, float radius)
    {
        List<Collider> goColliders = new List<Collider>();
        List<GameObject> go = new List<GameObject>();

        goColliders.AddRange(Physics.OverlapSphere(origin, radius, layer));

        foreach (Collider collider in goColliders)
        {
            go.Add(collider.gameObject);
        }

        return go;
    }
}
