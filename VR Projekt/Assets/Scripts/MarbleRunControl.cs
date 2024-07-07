using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Überprüft alle Räder der Murmelbahnauf Korrektheit
/// </summary>
public class MarbleRunControl : MonoBehaviour
{
    [SerializeField]
    [Tooltip("All wheels of the marbleRun")]
    MarbleRun[] wheels; 

    public bool allCorrect = false;

    public UnityEvent cleared;

    // Checkt ob alle Räder in der richtigen Position sind
    public void checkCombination()
    {
        allCorrect = true;

        // Überprüfe jede Rad-Instanz, ob sie richtig eingestellt ist
        foreach (MarbleRun wheel in wheels)
        {
            if (!wheel.isCorrect)
            {
                allCorrect = false; 
                break;
            } 
        }
         
        // Wenn alle Räder korrekt eingestellt sind, gib eine Nachricht aus
        if (allCorrect)
        {
            foreach (MarbleRun wheel in wheels)
            {
                wheel.setCorrectColor();
            }

            cleared.Invoke();
        }
        else
        {
            foreach (MarbleRun wheel in wheels)
            {
                wheel.resetValue();
            }
        }
    }
}
