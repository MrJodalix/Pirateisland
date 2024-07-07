using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skript zum bewegen des Objektes in Richtung einer Stopp-Position
/// </summary>
public class MarbleMap : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Bewegt Objekt in die Richtung X")]
    bool moveX = false;
    [SerializeField]
    [Tooltip("Bewegt Objekt in die Richtung Y")]
    bool moveY = false;
    [SerializeField]
    [Tooltip("Bewegt Objekt in die Richtung Z")]
    bool moveZ = false;

    [SerializeField]
    [Tooltip("Stop-Position des Objekts")]
    Vector3 stopPosition;

    [SerializeField]
    [Tooltip("Geschwindigkeitsvektor")]
    Vector3 speed = new Vector3(0.0f, 1.0f, 0.0f);

    // Gibt an ob das Objekt sich bewegt
    private bool move = false;

    // Gibt an ob die Stopp-Position erreicht wurde
    private bool finished = false;


    // Verschiebt das Objekt bis zur Stopp-Position, wenn move auf True gesetzt wird
    void Update()
    {
        if (move && !finished) {
            transform.position += speed * Time.deltaTime;
            Vector3 newPos = transform.position;
            if (moveX)
                newPos.x += speed.x * Time.deltaTime;
            if (moveY)
                newPos.y += speed.y * Time.deltaTime;
            if (moveZ)
                newPos.z += speed.z * Time.deltaTime;

            transform.position = newPos;

            moveX = (moveX && ((speed.x < 0 && transform.position.x >= stopPosition.x) || (speed.x > 0 && transform.position.x <= stopPosition.x)));
            moveY = (moveY && ((speed.y < 0 && transform.position.y >= stopPosition.y) || (speed.y > 0 && transform.position.y <= stopPosition.y)));
            moveZ = (moveZ && ((speed.z < 0 && transform.position.z >= stopPosition.z) || (speed.z > 0 && transform.position.z <= stopPosition.z)));

            finished = !moveX && !moveY && !moveZ;
        }
    }

    // Startet die Bewegung des Objekts
    public void setMoveTrue()
    {
        move = true;
    }
}
