using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Vector3 center;
    public float radius;
    public Vector3 GetRandomPos() {
        float x = UnityEngine.Random.Range(-radius, radius);
        float z = UnityEngine.Random.Range(-radius, radius);
        return center + new Vector3(x, 0, z);
    }
}
