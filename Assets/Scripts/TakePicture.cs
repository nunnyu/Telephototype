using System;
using UnityEngine;

public class TakePicture : MonoBehaviour
{
    [Header("This script should be attached to an empty parent.")]
    [SerializeField] private GameObject defaultPose;
    [SerializeField] private GameObject cameraPoseUp;
    [SerializeField] private GameObject cameraPoseDown;
    [SerializeField] private GameObject cameraPoseLeft;
    [SerializeField] private GameObject cameraPoseRight;

    private GameObject cameraPose;
    private Vector2 lastPosition;
    private bool takingPic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultPose.SetActive(true);
        cameraPoseUp.SetActive(false);
        cameraPoseDown.SetActive(false);
        cameraPoseLeft.SetActive(false);
        cameraPoseRight.SetActive(false);

        cameraPose = cameraPoseDown; // Just a default, incase there are any weird bugs
        cameraPose.SetActive(false);

        takingPic = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChooseCameraPose();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!takingPic)
            {
                OnCameraInput();
                takingPic = true;
            }
        }
    }

    void ChooseCameraPose()
    {
        if (PlayerMovement.MovingUp)
        {
            cameraPose = cameraPoseUp;
        }
        else if (PlayerMovement.MovingDown)
        {
            cameraPose = cameraPoseDown;
        }
        else if (PlayerMovement.MovingLeft)
        {
            cameraPose = cameraPoseLeft;
        }
        else if (PlayerMovement.MovingRight)
        {
            cameraPose = cameraPoseRight;
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
