using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RockCircle : MonoBehaviour
{
    [SerializeField]
    [Tooltip("all missing stone sockets")]
    SocketControl[] sockets;


    public bool allCorrect = false;

    public UnityEvent cleared;
    
    public void checkAllSockets ()
    {
        allCorrect = true;

        // Überprüfe jede Rad-Instanz, ob sie richtig eingestellt ist
        foreach (SocketControl rock in sockets)
        {
            if (!rock.isCorrect)
            {
                allCorrect = false; // Wenn ein Rad falsch ist, ist die gesamte Kombination falsch
                break;
            }
        }

        // Wenn alle Räder korrekt eingestellt sind, gib eine Nachricht aus
        if (allCorrect)
        {
            cleared.Invoke();
        } 
    }
}
