using UnityEngine;

public class LocklingBehavior : MonoBehaviour
{
    public int enemiesNeeded = 5;
    public int dialogueDelay = 1;
    public GameObject lockling;
    public GameObject dialogue;
    private GameManager game;
    private bool disabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null) game = GameManager.Instance;
        game.OnReset.AddListener(Reset);
    }

    void Reset()
    {
        // Lowkey no reason to re-enable the lockling, as once this is done it should be permanent 
        // lockling.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.spiritsCaptured == enemiesNeeded && !disabled)
        {
            Invoke("HideLockling", dialogueDelay + .3f);
            Invoke("MakeDialogue", dialogueDelay);
            disabled = true;
        }
    }

    void HideLockling()
    {
        lockling.SetActive(false);
    }

    void MakeDialogue()
    {
        Instantiate(dialogue);
    }
}
