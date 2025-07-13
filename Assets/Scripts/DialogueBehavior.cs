using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            DialogueManager.ShowDialogue = true;
        }
    }
}
