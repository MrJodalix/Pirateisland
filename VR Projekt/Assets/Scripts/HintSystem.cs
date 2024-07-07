using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class HintSystem : MonoBehaviour
{
    public RockCircle rockCircle;
    public MarbleRunControl marbleRun;
    public DoorController prisonDoor;
    public TreeController tree;
    public ChestUpperPartController chest;

    private bool firstHint = true;

    private bool waitTimer = false;


    public void playHint()
    {
        if (!waitTimer)
        {
            if (firstHint)
            {
                AudioManager.instance.Play("FirstHint");
                Debug.Log("FirstHint Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
                firstHint = false;
            }
            else if (!rockCircle.allCorrect)
            {
                AudioManager.instance.Play("RockCircleHint");
                Debug.Log("RockCircle Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
            }
            else if (!marbleRun.allCorrect)
            {
                AudioManager.instance.Play("MarbleRunHint");
                Debug.Log("Marble Run Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
            }
            else if (!prisonDoor.isOpen)
            {
                AudioManager.instance.Play("PrisonDoorHint");
                Debug.Log("Prison Door Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
            }
            else if (!chest.isOpen)
            {
                AudioManager.instance.Play("ChestHint");
                Debug.Log("Chest Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
            }
            else if (!tree.isChopped)
            {
                AudioManager.instance.Play("TreeHint");
                Debug.Log("Tree Hint Played");
                StartCoroutine(waitCoroutine(5.0f));
            }
        }

    }

    IEnumerator waitCoroutine(float wait)
    {
        waitTimer = true;
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(wait);
        waitTimer = false;
    }

}
