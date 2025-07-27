using UnityEngine;

public class ChudAnimationController : MonoBehaviour
{
    [Header("This script is for testing purposes only")]
    [Header("Left Sprite")]
    public GameObject leftAnim;

    [Header("Idle Sprite")]
    public GameObject idleChud;

    [Header("Right Sprite")]
    public GameObject rightAnim;

    [Header("Input Settings")]
    public KeyCode rightKey = KeyCode.D;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode idleKey = KeyCode.W;

    void Start()
    {
        leftAnim.SetActive(false);
        idleChud.SetActive(true);
        rightAnim.SetActive(false);
    }
    void Update()
    {
        // Check for keyboard input
        if (Input.GetKeyDown(leftKey))
        {
            leftAnim.SetActive(true);
            idleChud.SetActive(false);
            rightAnim.SetActive(false);

        }
        else if (Input.GetKeyDown(idleKey))
        {
            leftAnim.SetActive(false);
            idleChud.SetActive(true);
            rightAnim.SetActive(false);
        }
        else if (Input.GetKeyDown(rightKey))
        {
            leftAnim.SetActive(false);
            idleChud.SetActive(false);
            rightAnim.SetActive(true);
        }
    }

}