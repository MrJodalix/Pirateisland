using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : MonoBehaviour
{
    public bool isCorrect = false;
    /*
    [SerializeField]
    [Tooltip("Min Correct Angle")]
    float minAngle = 0.0f;

    [SerializeField]
    [Tooltip("Max Correct Angle")]
    float maxAngle = 0.0f;
    */
    [SerializeField]
    [Tooltip("Lockcontroller for the dial")]
    LockControl lockControl = null;    

    public void testAngle (float angle)
    {   
        
        angle = transform.localEulerAngles.y;
        //Debug.Log(angle);
        //isCorrect = (angle >= minAngle && angle <= maxAngle);
        isCorrect = ((angle >= 0.0f && angle <= 20.0f) || ( angle >= 340.0f && angle <= 360.0f));
        if (lockControl != null)
            lockControl.checkCombination();
    }

}