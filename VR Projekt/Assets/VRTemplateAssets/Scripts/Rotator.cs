using System;
using System.Diagnostics;
using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Rotates this object at a user defined speed
    /// </summary>
    public class Rotator : MonoBehaviour
    {
        [SerializeField, Tooltip("Angular velocity in degrees per second")]
        Vector3 m_Velocity;
        public Vector3 minAngle;
        public Vector3 maxAngle;

        void Update()
        {
            transform.Rotate(m_Velocity * Time.deltaTime);
            

            Vector3 rotaion = transform.rotation.eulerAngles;
            rotaion.x = rotaion.x < minAngle.x ? minAngle.x : rotaion.x;
            rotaion.y = rotaion.y < minAngle.y ? minAngle.y : rotaion.y;
            rotaion.z = rotaion.z < minAngle.z ? minAngle.z : rotaion.z;

            rotaion.x = rotaion.x > maxAngle.x ? maxAngle.x : rotaion.x;
            rotaion.y = rotaion.y > maxAngle.y ? maxAngle.y : rotaion.y;
            rotaion.z = rotaion.z > maxAngle.z ? maxAngle.z : rotaion.z;

            transform.rotation = Quaternion.Euler(rotaion);
            //UnityEngine.Debug.Log("Hallo: " + rotaion.y + " - " + transform.rotation.eulerAngles.y);
        }
    }
}
