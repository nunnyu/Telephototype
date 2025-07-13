using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private Sprite sprite;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            DialogueManager.ShowDialogue = true;
            FindFirstObjectByType<DialogueManager>().SetText(text);
            FindFirstObjectByType<DialogueManager>().SetSprite(sprite);
        }
    }
}
