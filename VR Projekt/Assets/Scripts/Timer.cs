using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public GameObject leftHandRayIndicator;
    public GameObject rightHandRayIndicator;
    public GameObject endMenu;
    public GameObject directionalLight;

    private float totalTime;

    private void Start()
    {
        totalTime = remainingTime; // Store the initial total time
    }

    void Update()
    {
        if(remainingTime > 0) 
        {
            remainingTime -= Time.deltaTime;
        }

        else if(remainingTime < 0)
        {
            remainingTime = 0;
            endMenu.SetActive(true);
            leftHandRayIndicator.SetActive(true);
            rightHandRayIndicator.SetActive(true);

        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Calculate the rotation based on the remaining time
        float rotationAngle = Mathf.Lerp(-2f, 180f, remainingTime / totalTime);
        directionalLight.transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
