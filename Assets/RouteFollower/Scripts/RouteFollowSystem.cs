using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteFollowSystem : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private float _cameraRotateSpeed = 5f;
    [SerializeField] private Transform route;
    [SerializeField] private bool _loopMap;
    private int routeToGo;
    private float tParam;
    private Vector3 playerPosition;
    [SerializeField] private float speedModifier = 0.5f;
    private bool coroutineAllowed;

    private void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        coroutineAllowed = true;
    }

    private void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNumber)
    {
        coroutineAllowed = false;
        if (routeNumber == route.childCount - 1)
        {
            if (_loopMap)
            {
                routeToGo = 0;
                coroutineAllowed = true;
            }
            yield break;
        }
        Vector3 p0 = route.GetChild(routeNumber).position;
        Vector3 p1 = route.GetChild(routeNumber).GetChild(0).position;
        Vector3 p2 = route.GetChild(routeNumber + 1).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            playerPosition = CalculateArc(tParam, p0, p1, p2);

            var rotation = Quaternion.LookRotation(playerPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _cameraRotateSpeed);

            transform.position = playerPosition;

            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > route.childCount - 1 == false)
        {
            coroutineAllowed = true;
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