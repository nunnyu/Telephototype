using System;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    [Header("A global dialogue just happens - no colliders, so these should be instantiated by an object.")]
    [SerializeField] private bool global = false;

    [Header("Should this dialogue be destroyed after reading it?")]
    [SerializeField] private bool toDestroy = true;

    [Header("Automatically close this dialogue after a bit?")]
    [SerializeField] private bool automaticDestruction = false;

    [Header("Customize icons & text")]
    [SerializeField] private Dialogue[] dialogues;
    private int current;
    private bool inDialogue = false;

    void HandleDialogue(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inDialogue = true;
            current = 0;
            DialogueManager.ShowDialogue = true;
            SetDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!global)
            HandleDialogue(other);
    }

    void Start()
    {
        current = 0;

        if (global)
        {
            // We don't really care about collisions we are just showing a dialogue because something happened.
            inDialogue = true;
            SetDialogue();
        }

        if (automaticDestruction)
        {
            FindFirstObjectByType<DialogueManager>().DelayedDestruction(1);
        }
    }

    void SetDialogue()
    {
        FindFirstObjectByType<DialogueManager>().SetText(dialogues[current].text);
        FindFirstObjectByType<DialogueManager>().SetSprite(dialogues[current].sprite);
        FindFirstObjectByType<DialogueManager>().SetAudio(dialogues[current].audio);
    }

    void Update()
    {
        Debug.Log(inDialogue);

        if (inDialogue)
        {
            // There was this ugly annoying bug, where automatic destruction makes 
            // walking into another dialogue really funky - as it tries to hide it in the middle of 
            // your new dialogue. 
            DialogueManager.ShowDialogue = true;
            if (Input.GetKeyDown(KeyCode.E) && !automaticDestruction)
            {
                current++;
                if (current == dialogues.Length)
                {
                    End();
                    return;
                }
                SetDialogue();
                FindFirstObjectByType<DialogueManager>().PlayClip();
            }
        }
    }

    void End()
    {
        inDialogue = false;
        DialogueManager.ShowDialogue = false;

        if (toDestroy)
        {
            Destroy(this.gameObject);
            current = 0;
        }
    }
}
