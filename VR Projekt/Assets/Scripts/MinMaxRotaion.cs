using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector3 minAngle;
    public Vector3 maxAngle;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotaion = transform.rotation.eulerAngles;
        rotaion.x = rotaion.x < minAngle.x ? minAngle.x : rotaion.x;
        rotaion.y = rotaion.y < minAngle.y ? minAngle.y : rotaion.y;
        rotaion.z = rotaion.z < minAngle.z ? minAngle.z : rotaion.z;

        rotaion.x = rotaion.x < maxAngle.x ? maxAngle.x : rotaion.x;
        rotaion.y = rotaion.y < maxAngle.y ? maxAngle.y : rotaion.y;
        rotaion.z = rotaion.z < maxAngle.z ? maxAngle.z : rotaion.z;

        transform.rotation = Quaternion.Euler(rotaion);
        Console.WriteLine("%f %f", transform.rotation.eulerAngles.y, rotaion.y);
    }
}
