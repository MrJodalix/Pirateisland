using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUpperPartController : MonoBehaviour
{
    public GameObject movableLid;
    public bool isOpen = false;

   public void openChest()
   {
        movableLid.SetActive(true);
        gameObject.SetActive(false);
        isOpen = true;
   }
}
