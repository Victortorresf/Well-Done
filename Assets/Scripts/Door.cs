using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen =  false;

    public void OpenDoor()
    {
        gameObject.SetActive(false);
        isOpen = true;
    }

}
