using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private Transform[] controlPoints;

    private Vector3 gizmozPosition;
    public float RunSpeed = 0.05f;

    private void OnDrawGizmos()
    {
        if (RunSpeed < 0.01f)
        {
            return;
        }

        for (int i = 0; i < controlPoints.Length - 1; i++)
        {
            for (float t = 0; t < 1; t += RunSpeed)
            {
                gizmozPosition = CalculateArc(t, controlPoints[i].position, controlPoints[i].GetChild(0).position,
                    controlPoints[i + 1].position);
                Gizmos.DrawSphere(gizmozPosition, 0.25f);
            }
        }
    }

    private Vector3 CalculateArc(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}