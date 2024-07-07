using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Test ob eine Scheibe in die richtige Richtung gedreht ist und passt die Farbe an
/// </summary>
public class MarbleRun : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Muss rechtsrum rotiert werden, Standard maessig links herum")]
    bool rightRotation = false;

    [SerializeField]
    [Tooltip("Für ein fake Rad")]
    public bool alwaysCorrect = false;

    [SerializeField]
    [Tooltip("Ring der die Farbe ändert")]
    public GameObject colorRing;

    [SerializeField]
    [Tooltip("Base Material")]
    public Material startMat;

    [SerializeField]
    [Tooltip("Material wenn correct")]
    public Material correctMat;

    [SerializeField]
    [Tooltip("Material wenn falsch")]
    public Material falseMat;
    public bool isCorrect = false;
    public UnityEvent onAllFalse;

    // testet die Rotaion
    public void testValue(float value)
    {    
        if ((rightRotation && value >= 0.95f) || (!rightRotation && value <= 0.05f || alwaysCorrect)) {
            isCorrect = true;
        } else {
            isCorrect = false;
        }
    }

    // Setzt die Richtige Farbe
    public void setCorrectColor ()
    {
        if (colorRing != null)
            colorRing.GetComponent<Renderer>().material = correctMat;
    }

    // Setzt die Werte zurück, bei falscher Rotation
    public void resetValue ()
    {
        onAllFalse.Invoke();
        if (colorRing != null){
            StartCoroutine(colorCoroutine());
        }
    }

    // Wartet Zeit ab bis man wieder testen kann
    IEnumerator colorCoroutine()
    {
        colorRing.GetComponent<Renderer>().material = falseMat;
        yield return new WaitForSeconds(5);
        colorRing.GetComponent<Renderer>().material = startMat;
    }
}
