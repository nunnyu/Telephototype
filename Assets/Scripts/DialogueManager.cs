using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] public GameObject dialogueBox;
    [SerializeField] private GameObject text;
    [SerializeField] private Image icon;
    [SerializeField] private AudioClip clip;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float hidePositionOffset = -300;
    [SerializeField] private float shownPositionOffset = 50;
    private Vector3 targetPos;
    private Vector3 hidePosition;
    private Vector3 shownPosition;
    public static bool ShowDialogue;
    private bool playingAudio = false;

    public void SetText(string txt)
    {
        text.GetComponent<TMP_Text>().text = txt;
    }

    public void SetSprite(Sprite sprite)
    {
        icon.sprite = sprite;
    }

    public void SetAudio(AudioClip clip)
    {
        this.clip = clip;
    }

    public void DelayedDestruction(float delay)
    {
        Invoke("HideDialogue", delay);
    }

    // No references, but invoked for delayed destruction 
    void HideDialogue()
    {
        ShowDialogue = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetText("hello.");

        hidePosition = new Vector3(dialogueBox.transform.position.x, hidePositionOffset);
        shownPosition = new Vector3(dialogueBox.transform.position.x, shownPositionOffset);

        targetPos = hidePosition;
        dialogueBox.transform.position = hidePosition;
    }

    // Update is called once per frame
    void Update()
    {
        dialogueBox.transform.position = Vector3.Lerp(dialogueBox.transform.position, targetPos, speed * Time.deltaTime);

        if (ShowDialogue)
        {
            targetPos = shownPosition;
            PlayerMovement.DialogueLock = true;
            TakePicture.CanTakePicture = false;
            if (!playingAudio)
            {
                PlayClip();
                playingAudio = true;
            }
        }
        else
        {
            TakePicture.CanTakePicture = true;
            PlayerMovement.DialogueLock = false;
            targetPos = hidePosition;
            playingAudio = false;
        }
    }

    public void PlayClip()
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
}
