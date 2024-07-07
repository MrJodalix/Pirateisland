using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coconut_controler : MonoBehaviour
{
    [SerializeField]
    [Tooltip("static coconuts")]
    public GameObject[] staticCoconuts;

    [SerializeField]
    [Tooltip("falling coconuts")]
    public GameObject[] fallingCoconuts;

    [SerializeField]
    [Tooltip("offset der Bewegung in %")]
    float offset = 50;

    [SerializeField]
    [Tooltip("Zeitinterval zum schütteln in sec")]
    float timeInterval = 2.0f;

    float maxMovment = 0.0f;

    float minMovment = 0.0f;

    float timer = 0.0f;

    bool startTimer = false;

    float nextMovement = 0.0f;

    int coconutAmount = 0;

    int coconutCount = 0;
 
    void Start()
    {
        coconutAmount = fallingCoconuts.Length <= staticCoconuts.Length ? fallingCoconuts.Length : staticCoconuts.Length;
        // Hole die HingeJoint-Komponente des GameObjects
        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        offset = (100 - offset) / 100;


        // Überprüfe, ob eine HingeJoint-Komponente vorhanden ist
        if (hingeJoint != null)
        {
            // Gib die Min- und Max-Limits des HingeJoint aus
            minMovment = hingeJoint.limits.min * offset;
            maxMovment = hingeJoint.limits.max * offset;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (coconutAmount > coconutCount){
            float currRotation = transform.eulerAngles.x;
        
            if (currRotation > 180 )
                currRotation -= 360;

            bool isSmallerAngle = currRotation <= minMovment;  
            bool isBiggerAngle = currRotation >= maxMovment;

            if (!startTimer && (isSmallerAngle || isBiggerAngle))
            {
                startTimer = true;
                nextMovement = isSmallerAngle ? maxMovment : minMovment;
            }
                
            if (startTimer) {
                timer += Time.deltaTime;
                if ((nextMovement < 0.0f && currRotation <= nextMovement) || 
                    (nextMovement > 0.0f && currRotation >= nextMovement))
                {
                    staticCoconuts[coconutCount].SetActive(false);
                    fallingCoconuts[coconutCount].SetActive(true);
                    coconutCount++;
                    startTimer = false;
                    timer = 0.0f;
                } else if (timer >= timeInterval){
                    startTimer = false;
                    timer = 0.0f;
                }
            }
        }
    }
}
