using System;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    [SerializeField] private bool toDestroy = true;
    [SerializeField] private Dialogue[] dialogues;
    private int current;
    private bool inDialogue = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        inDialogue = true;
        current = 0;
        if (other.tag == "Player")
        {
            DialogueManager.ShowDialogue = true;
            SetDialogue();
        }
    }

    void SetDialogue()
    {
        FindFirstObjectByType<DialogueManager>().SetText(dialogues[current].text);
        FindFirstObjectByType<DialogueManager>().SetSprite(dialogues[current].sprite);
    }

    void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Space))
            {
                current++;
                if (current == dialogues.Length)
                {
                    End();
                    return;
                }
                SetDialogue();
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
