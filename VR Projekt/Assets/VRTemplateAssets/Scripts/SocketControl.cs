using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketControl : MonoBehaviour
{
    public XRSocketInteractor socket;

    public string correctTag;
    public bool isCorrect;


    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    public void isRock()
    {
        if (socket != null){
            IXRSelectInteractable obj = socket.GetOldestInteractableSelected();
            isCorrect = obj.transform.gameObject.CompareTag(correctTag);
        }
        
    }
    public void isEmpty()
    {
        isCorrect = false;
    }
}
