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

    [Header("Sound")]
    [SerializeField] private AudioClip snapSound;
    [SerializeField] private float soundDelay = .5f;

    private GameObject cameraPose;
    private Vector2 lastPosition;
    public static bool CanTakePicture;
    public static bool TakingPic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanTakePicture = true;
        defaultPose.SetActive(true);
        cameraPoseUp.SetActive(false);
        cameraPoseDown.SetActive(false);
        cameraPoseLeft.SetActive(false);
        cameraPoseRight.SetActive(false);

        cameraPose = cameraPoseDown; // Just a default, incase there are any weird bugs
        cameraPose.SetActive(false);

        TakingPic = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChooseCameraPose();

        if (Input.GetKeyDown(KeyCode.Space) && CanTakePicture)
        {
            if (!TakingPic)
            {
                OnCameraInput();
                TakingPic = true;
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
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = snapSound;
        audioSource.PlayDelayed(soundDelay);

        lastPosition = defaultPose.transform.position;
        defaultPose.SetActive(false);

        cameraPose.SetActive(true);
        // cameraPose.transform.position = lastPosition;

        Vector3 targetPos = new Vector3(lastPosition.x, lastPosition.y - cameraPose.GetComponent<OnPicture>().initialDisplacement);
        cameraPose.transform.position = targetPos;

        Invoke("Over", 1f);
    }

    void Over()
    {
        TakingPic = false;
        cameraPose.SetActive(false);

        // Visual bug fix
        defaultPose.transform.localScale = new Vector3(1, 1, 1);
        defaultPose.GetComponent<PlayerMovement>().ResetLimbs();

        // Back to default idle pose
        defaultPose.SetActive(true);
    }
}
