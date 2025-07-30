using UnityEngine;

public class SpawnDeathDialogue : MonoBehaviour
{
    public GameObject dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyHealth>().GetHealth() == 0)
        {
            Invoke("SpawnDialogue", 1);
        }
    }

    void SpawnDialogue()
    {
        Instantiate(dialogue);
    }
}
