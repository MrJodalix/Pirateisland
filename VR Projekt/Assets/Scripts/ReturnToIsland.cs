using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToIsland : MonoBehaviour
{
    protected class ObjectInfo
    {
        public Collider Collider;
        public Vector3 StartPosition;
        public Vector3 MiddlePosition;
        public Vector3 EndPosition;
        public float Animation;
        public bool MovingToMiddle;

        public ObjectInfo(Collider collider, Vector3 startPosition, Vector3 middlePosition, Vector3 endPosition)
        {
            Collider = collider;
            StartPosition = startPosition;
            MiddlePosition = middlePosition;
            EndPosition = endPosition;
            Animation = 0;
            MovingToMiddle = true;
        }
    }

    protected List<ObjectInfo> objects = new List<ObjectInfo>();
    protected float returnSpeed = 0.5f;
    protected float platformSize = 18f; // Assuming a 20x20 platform

    // OnTriggerEnter is called if a collider enters the collider of the object
    private void OnTriggerEnter(Collider other)
    {
        Vector3 startPosition = other.gameObject.transform.position;
        Vector3 middlePosition = new Vector3(startPosition.x, 0.0f, startPosition.z);
        Vector3 endPosition = CalculateEdgePosition(startPosition);

        ObjectInfo objectInfo = new ObjectInfo(other, startPosition, middlePosition, endPosition);
        objects.Add(objectInfo);

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    private Vector3 CalculateEdgePosition(Vector3 position)
    {
        float halfSize = platformSize / 2;
        float edgeX = Mathf.Clamp(position.x, -halfSize, halfSize);
        float edgeZ = Mathf.Clamp(position.z, -halfSize, halfSize);

        return new Vector3(edgeX, 0.0f, edgeZ);
    }

    
    private void OnTriggerExit(Collider other)
     {
        // Instead of directly removing the object from the list here, we only re-enable gravity
        foreach (var obj in objects)
        {
            if (obj.Collider == other)
            {
                Rigidbody rb = obj.Collider.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                }
                // Do not remove from the list here
                break;
            }
        }
    }
    

    private void Update()
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            ObjectInfo obj = objects[i];
            obj.Animation += Time.deltaTime * returnSpeed;

            if (obj.MovingToMiddle)
            {
                // First stage: Interpolate to y = 0
                Vector3 newPosition = Vector3.Lerp(obj.StartPosition, obj.MiddlePosition, obj.Animation);
                obj.Collider.gameObject.transform.position = newPosition;

                //Debug.Log($"Moving to middle: {newPosition}");

                // Check if the object has reached the middle position (y = 0)
                if (Vector3.Distance(newPosition, obj.MiddlePosition) < 0.1f)
                {
                    obj.MovingToMiddle = false; // Switch to moving to the edge
                    obj.StartPosition = obj.MiddlePosition; // Update start position for next stage
                    obj.Animation = 0; // Reset animation for next stage

                    //Debug.Log("Reached middle position, moving to edge");
                }
            }
            else
            {
                // Second stage: Interpolate to the edge of the platform
                Vector3 newPosition = Vector3.Lerp(obj.StartPosition, obj.EndPosition, obj.Animation);
                obj.Collider.gameObject.transform.position = newPosition;

                //Debug.Log($"Moving to edge: {newPosition}");

                // Check if the object has reached the edge
                if (Vector3.Distance(newPosition, obj.EndPosition) < 0.1f)
                {
                    Rigidbody rb = obj.Collider.gameObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = false;
                    }
                    objects.RemoveAt(i); // Remove the object from the list

                    //Debug.Log("Reached edge position, reset object");
                }
            }
        }
    }
}
