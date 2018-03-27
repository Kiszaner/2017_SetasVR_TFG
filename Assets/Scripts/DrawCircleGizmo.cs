using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircleGizmo : MonoBehaviour
{
    public float Radius = 4;

    private void OnDrawGizmosSelected()
    {
        Transform t = GetComponent<Transform>();
        Gizmos.color = Color.white;
        float theta = 0;
        float x = Radius * Mathf.Cos(theta);
        float y = Radius * Mathf.Sin(theta);
        Vector3 pos = t.position + new Vector3(x, 0, y);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;
        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = Radius * Mathf.Cos(theta);
            y = Radius * Mathf.Sin(theta);
            newPos = t.position + new Vector3(x, 0, y);
            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos, lastPos);
    }
}
