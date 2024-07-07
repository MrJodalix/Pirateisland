using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePlaneCollider : MonoBehaviour
{
    private int indexOfCollider = 0;

    GameObject splinePlane;

    public int maxLowering = 2;

    private int loweringCount = 0;

    public void setConnection (int index, GameObject plane){
        indexOfCollider = index;
        splinePlane = plane;
    }

    void OnTriggerEnter(Collider other)
    {
        if (loweringCount < maxLowering)
        {
            splinePlane.GetComponent<GeneratePlaneMesh>().lowerPoints(indexOfCollider);
            loweringCount++;
        }
            
    }

    public void OnHit()
    {
        if (loweringCount < maxLowering)
        {
            splinePlane.GetComponent<GeneratePlaneMesh>().lowerPoints(indexOfCollider);
            loweringCount++;
        }
            
    }
}
