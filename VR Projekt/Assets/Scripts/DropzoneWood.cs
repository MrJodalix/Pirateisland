
using System;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class DropzoneWood : MonoBehaviour
{
    [SerializeField]
    [Tooltip("static wood")]
    public GameObject staticWood;

    [SerializeField]
    [Tooltip("falling woods")]
    private GameObject[] fallingWood;

    int woodCount = 0;

    public Collider DropzoneCollider;

    public MeshRenderer meshRender;

    public XRSocketInteractor socket;
    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (woodCount < 1)
        {
            Debug.Log(socket.hasSelection);
            Debug.Log(socket.isPerformingManualInteraction);
            if (socket.hasSelection)
            {
                foreach (var item in fallingWood)
                {
                    if (socket)
                    {
                       
                    }
                    Debug.Log("WoodCount");
                }

            }
            //if (DropzoneCollider.isTrigger && other.gameObject.CompareTag("Wood"))
            //{
            //    meshRender.enabled = false;
            //    woodCount++;
            //    Debug.Log("Set");
            //}
        }
    }
}