using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeHolder : MonoBehaviour
{
    public GameObject brokenHold;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Axe")) 
        {
            brokenHold.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
