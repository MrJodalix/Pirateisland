using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampColor : MonoBehaviour
{
    public Material newMat;

    public Material oldMat;

    void Start()
    {
        GetComponent<Renderer>().material = oldMat;
    }

    public void changeColor(bool unlocked)
    {
        if (unlocked)
        {
            GetComponent<Renderer>().material = newMat;
        }
        else
        {
            GetComponent<Renderer>().material = oldMat;
        }
    }

}
