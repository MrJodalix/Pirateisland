using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScreen : MonoBehaviour
{
    public GameObject finishScreen;
    public GameObject boat;

    void OnEnable()
    {
        if(boat.activeSelf == true)
        {
            finishScreen.SetActive(true);
        }
    }

}
