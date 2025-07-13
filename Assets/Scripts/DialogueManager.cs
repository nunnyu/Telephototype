using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float hidePositionOffset = -300;
    [SerializeField] private float shownPositionOffset = 50;
    private Vector3 targetPos;
    private Vector3 hidePosition;
    private Vector3 shownPosition;
    public static bool ShowDialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hidePosition = new Vector3(dialogueBox.transform.position.x, hidePositionOffset);
        shownPosition = new Vector3(dialogueBox.transform.position.x, shownPositionOffset);

        targetPos = hidePosition;
        dialogueBox.transform.position = hidePosition;
    }

    // Update is called once per frame
    void Update()
    {
        dialogueBox.transform.position = Vector3.Lerp(dialogueBox.transform.position, targetPos, speed * Time.deltaTime);

        // For Testing Purposes
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShowDialogue = true;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowDialogue = false;
        }

        if (ShowDialogue)
        {
            targetPos = shownPosition;
            PlayerMovement.DialogueLock = true;
            TakePicture.CanTakePicture = false;
        }
        else
        {
            TakePicture.CanTakePicture = true;
            PlayerMovement.DialogueLock = false;
            targetPos = hidePosition;
        }
    }
}
