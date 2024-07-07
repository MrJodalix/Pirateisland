using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class LockControl : MonoBehaviour
{
    [SerializeField]
    [Tooltip("All dials of the lock")]
    Dial[] dials; // Eine Array-Referenz auf alle Rad-Skripte
    public DoorController doorController;
    public UnityEvent doorOpen;



    public LampColor lampColor;

    public void checkCombination()
    {
        bool allCorrect = true;

        // Überprüfe jede Rad-Instanz, ob sie richtig eingestellt ist
        foreach (Dial dial in dials)
        {
            if (!dial.isCorrect)
            {
                allCorrect = false; // Wenn ein Rad falsch ist, ist die gesamte Kombination falsch
                break;
            }
        }

        // Wenn alle Räder korrekt eingestellt sind, gib eine Nachricht aus
        if (allCorrect)
        {
            lampColor.changeColor(true);
            doorController.openDoor();
            doorOpen.Invoke();

        } else {
            lampColor.changeColor(false);
        }
    }
}
