using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHive : MonoBehaviour
{
    public bool playerNear =  false;
    private void OnTriggerEnter(Collider other)
    {
        playerNear = true;
    }
    private void OnTriggerExit(Collider other)
    {
        playerNear = false;
    }
}
