using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Vector3 centerA,centerB;
    public float length,width;
    public Vector3 GetRandomPos(ETeam eTeam) {
        float x = UnityEngine.Random.Range(-length / 2, length / 2);
        float z = UnityEngine.Random.Range(-width / 2, width / 2);
        if (eTeam == ETeam.TeamA)
        {
            return centerA + new Vector3(x, 0, z);
        }
        else {
            return centerB + new Vector3(x, 0, z);
        }
    }
    private void OnDrawGizmos()
    {
        DrawRect(true);
        DrawRect(false);
    }
    void DrawRect(bool a_b)
    {
        Vector3 p = centerA;
        if (!a_b) {
            p = centerB;
        }
        p.y = 0.2f;
        Vector3 v0 = p;
        v0.x -= length / 2;
        v0.z += width / 2;

        Vector3 v1 = p;
        v1.x += length / 2;
        v1.z += width / 2;

        Vector3 v2 = p;
        v2.x += length / 2;
        v2.z -= width / 2;

        Vector3 v3 = p;
        v3.x -= length / 2;
        v3.z -= width / 2;

        if (a_b)
        {
            Gizmos.color = Color.red;
        }
        else {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawLine(v0, v1);
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v0);
    }
}
