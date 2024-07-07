using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    AudioSource sound;
    bool isPressed;

    private Vector3 startPos;

    public float waitTime = 5.0f;

    private bool waitTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        sound = GetComponent<AudioSource>();
        startPos = button.transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed && !waitTimer) 
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            sound.Play();
            isPressed = true;
            StartCoroutine(waitCoroutine(waitTime));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = startPos;
            onRelease.Invoke();
            isPressed = false;
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
