using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeSpline : MonoBehaviour
{
    public Collider spadeTipCollider;

    private Rigidbody spadeRb;
    public float breakThreshold = 0.1f;
    public GameObject chest;

    public bool isGrabed = false;

    public void setGrabed (bool selected){
        isGrabed = selected;
    }

    private void Start()
    {
        spadeRb = GetComponent<Rigidbody>();  // Get the Rigidbody of the current object (spade parent)
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the spade tip's collider
        if (isGrabed && collision.contacts[0].thisCollider == spadeTipCollider && (collision.gameObject.CompareTag("Sand")) )
        {
            // Calculate the angle of impact
            float impactAngleFactor = Vector3.Dot(-spadeTipCollider.transform.right, collision.contacts[0].normal);

            // Ensure angle factor is non-negative
            impactAngleFactor = Mathf.Abs(impactAngleFactor);

            // Calculate velocity of the spade's tip
            Vector3 tipVelocity = spadeRb.GetPointVelocity(spadeTipCollider.transform.position);

            // Modify the force based on the impact angle
            float modifiedForce = tipVelocity.magnitude * impactAngleFactor;


            // Check if the impact is strong enough to break the sand
            if (modifiedForce > breakThreshold)
            {
                collision.gameObject.GetComponent<SplinePlaneCollider>().OnHit();
            }

        }
    }
}
