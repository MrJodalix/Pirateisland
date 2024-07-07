using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject doorOpen;
    public bool isOpen = false;

    public void openDoor()
    {

        doorOpen.SetActive(true);
        gameObject.SetActive(false);
        isOpen = true;

    }
}
