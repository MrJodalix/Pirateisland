using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject shatteredTree; // Assign a pre-fractured Tree model
    public bool isChopped = false;

    public void BreakTree()
    {
        shatteredTree.SetActive(true);
        gameObject.SetActive(false);
        isChopped = true;
    }
}
