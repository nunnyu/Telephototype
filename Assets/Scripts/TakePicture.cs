using System;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [Header("This script should be attached to an empty parent.")]
    [SerializeField] private GameObject defaultPose;
    [SerializeField] private GameObject cameraPose;
    private Vector2 lastPosition;
    private Boolean takingPic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultPose.SetActive(true);
        cameraPose.SetActive(false);
        takingPic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!takingPic)
            {
                OnCameraInput();
                takingPic = true;
            }
        }
    }

    void OnCameraInput()
    {
        lastPosition = defaultPose.transform.position;
        defaultPose.SetActive(false);

        cameraPose.SetActive(true);
        cameraPose.transform.position = lastPosition;

        Invoke("Over", 1f);
    }

    void Over()
    {
        takingPic = false;
        cameraPose.SetActive(false);
        defaultPose.SetActive(true);
    }
}
