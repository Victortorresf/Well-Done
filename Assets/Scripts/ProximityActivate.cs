using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProximityActivate : MonoBehaviour
{

    public Transform distanceActivator, lookAt;
    public float distance;
    public Transform activator;
    public bool activeState = false;
    public GameObject target;

    bool IsTargetNear()
    {
        var distanceDelta = distanceActivator.position - activator.position;
        if (distanceDelta.sqrMagnitude < distance * distance)
        {
            if (lookAt != null)
            {
                var lookAtActivatorDelta = lookAt.position - activator.position;
                if (Vector3.Dot(activator.forward, lookAtActivatorDelta.normalized) > 0.95f)
                    return true;
            }
                return true;
        }
        return false;
    }

    void Update()
    {
        if (!activeState)
        {
            if (IsTargetNear())
            {
                target.SetActive(true);
                activeState = true;
            }
        }
        else
        {
            if (!IsTargetNear())
            {
                target.SetActive(false);
                activeState = false;
            }
        }
        
    }

}
